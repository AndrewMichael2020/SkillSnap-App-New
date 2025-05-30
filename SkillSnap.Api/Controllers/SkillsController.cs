using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly SkillSnapContext _context;
        private readonly IMemoryCache _cache;

        public SkillsController(SkillSnapContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Skill>), 200)]
        public async Task<IActionResult> GetSkills()
        {
            try
            {
                if (!_cache.TryGetValue("skills", out List<Skill>? skills))
                {
                    Console.WriteLine("Cache miss: fetching skills from DB.");
                    var stopwatch = Stopwatch.StartNew();
                    skills = await _context.Skills.AsNoTracking().ToListAsync();
                    stopwatch.Stop();
                    Console.WriteLine($"Skills request duration: {stopwatch.ElapsedMilliseconds}ms");
                    _cache.Set("skills", skills, TimeSpan.FromMinutes(5));
                }
                else
                {
                    Console.WriteLine("Skills cache hit: served from memory.");
                }
                return Ok(skills);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET /api/Skills: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving skills.");
            }
        }

        [HttpPost]
        public ActionResult<Skill> Add([FromBody] Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSkills), new { id = skill.Id }, skill);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteSkill(int id)
        {
            var skill = _context.Skills.Find(id);
            if (skill == null)
                return NotFound();

            _context.Skills.Remove(skill);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
