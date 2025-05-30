using SkillSnap.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SkillSnap.Client.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PortfolioUserDto?> GetPortfolioUserAsync(int id)
        {
            // Adjust endpoint as needed; assumes /api/users/{id}
            return await _httpClient.GetFromJsonAsync<PortfolioUserDto>($"{ApiRoutes.Users}/{id}");
        }
    }
}
