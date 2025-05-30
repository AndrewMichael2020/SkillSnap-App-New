using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using SkillSnap.Shared; // DTOs should be in SkillSnap.Shared

namespace SkillSnap.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task LoginAsync(LoginDto loginModel)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", loginModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<AuthResult>();
            if (result?.Token != null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
            }
        }

        public async Task RegisterAsync(RegisterDto registerModel)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/register", registerModel);
            response.EnsureSuccessStatusCode();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        private class AuthResult
        {
            public string? Token { get; set; }
        }
    }
}
