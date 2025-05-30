using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SkillSnap.Client.Services
{
    public class ProjectService
    {
        private readonly HttpClient _httpClient;

        public ProjectService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Project>>("/api/projects");
                return result ?? new List<Project>();
            }
            catch
            {
                // Optionally log the error here
                return new List<Project>();
            }
        }

        public async Task AddProjectAsync(Project newProject)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/projects", newProject);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to add project: {response.StatusCode} - {error}");
            }
        }
    }

    // Placeholder Project class; replace with shared DTO as needed
    public class Project
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Project name is required")]
        public string? Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public int PortfolioUserId { get; set; }
    }
}
