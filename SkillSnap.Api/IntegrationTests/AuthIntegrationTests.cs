using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using SkillSnap.Shared;
using SkillSnap.Api;
using System;

namespace SkillSnap.Api.IntegrationTests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Login_AuthorizeFlow_Works()
        {
            // Register user with unique email to avoid conflicts
            var uniqueEmail = $"testuser_{Guid.NewGuid()}@example.com";
            var registerDto = new RegisterDto { Email = uniqueEmail, Password = "Test123!" };
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            var registerContent = await registerResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Register response: {registerContent}");
            registerResponse.EnsureSuccessStatusCode();

            // Login user
            var loginDto = new LoginDto { Email = uniqueEmail, Password = "Test123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Login response: {loginContent}");
            loginResponse.EnsureSuccessStatusCode();
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResult>();
            Assert.False(string.IsNullOrEmpty(loginResult?.Token));

            // Attach JWT to Authorization header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            // Call [Authorize] route with token (should succeed or return 200/201/204)
            var postSkill = new { Name = "IntegrationTestSkill", Level = "Test" };
            var authResponse = await _client.PostAsJsonAsync("/api/skills", postSkill);
            var authContent = await authResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Authorized POST /api/skills response: {authContent}");
            Assert.True(authResponse.IsSuccessStatusCode);

            // Remove token and try again (should get 401)
            _client.DefaultRequestHeaders.Authorization = null;
            var unauthorizedResponse = await _client.PostAsJsonAsync("/api/skills", postSkill);
            var unauthContent = await unauthorizedResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Unauthorized POST /api/skills response: {unauthContent}");
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, unauthorizedResponse.StatusCode);
        }

        private class AuthResult
        {
            public string? Token { get; set; }
        }
    }
}
