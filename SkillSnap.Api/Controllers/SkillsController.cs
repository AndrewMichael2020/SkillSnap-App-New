using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly SkillSnapContext _context;

        public SkillsController(SkillSnapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Skill>> GetAll()
        {
            return Ok(_context.Skills.ToList());
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Skill> Add([FromBody] Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetAll), new { id = skill.Id }, skill);
        }

        [Authorize(Roles = "Admin")]
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
