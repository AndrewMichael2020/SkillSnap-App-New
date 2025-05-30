using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly SkillSnapContext _context;
        private readonly IMemoryCache _cache;

        public ProjectsController(SkillSnapContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            if (!_cache.TryGetValue("projects", out List<Project>? projects))
            {
                Console.WriteLine("Cache miss: fetching from DB.");
                var stopwatch = Stopwatch.StartNew();
                projects = await _context.Projects
                    .Include(p => p.Skills)
                    .AsNoTracking()
                    .ToListAsync();
                stopwatch.Stop();
                Console.WriteLine($"Request duration: {stopwatch.ElapsedMilliseconds}ms");
                _cache.Set("projects", projects, TimeSpan.FromMinutes(5));
            }
            else
            {
                Console.WriteLine("Cache hit: served from memory.");
            }
            return Ok(projects);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Project> Add(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            _cache.Remove("projects"); // Invalidate cache on modification
            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            _context.SaveChanges();
            _cache.Remove("projects"); // Invalidate cache on modification
            return NoContent();
        }
    }
}
