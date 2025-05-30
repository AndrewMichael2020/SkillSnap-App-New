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
using Microsoft.AspNetCore.Hosting;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly SkillSnapContext _context;
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _env;

        public ProjectsController(SkillSnapContext context, IMemoryCache cache, IWebHostEnvironment env)
        {
            _context = context;
            _cache = cache;
            _env = env;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjects()
        {
            // Remove this block to always allow anonymous access:
            // if (!_env.IsDevelopment() && !User.Identity.IsAuthenticated)
            // {
            //     return Unauthorized();
            // }
            try
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
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"Error in GET /api/Projects: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving projects.");
            }
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
