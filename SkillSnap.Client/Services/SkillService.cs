using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SkillSnap.Client.Services
{
    public class SkillService
    {
        private readonly HttpClient _httpClient;

        public SkillService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Skill>> GetSkillsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Skill>>("/api/skills");
            return result ?? new List<Skill>();
        }

        public async Task AddSkillAsync(Skill newSkill)
        {
            await _httpClient.PostAsJsonAsync("/api/skills", newSkill);
        }
    }

    // Placeholder Skill class; replace with shared DTO as needed
    public class Skill
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Level { get; set; }
        public int PortfolioUserId { get; set; }
    }
}
