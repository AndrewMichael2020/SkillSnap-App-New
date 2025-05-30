using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using SkillSnap.Shared;
using SkillSnap.Api;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;

namespace SkillSnap.Api.IntegrationTests
{
    public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _clientFactory;

        public AuthIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _clientFactory = factory;
            // Set JWT config for test environment if missing
            // Key must be at least 256 bits (32 chars for HS256)
            Environment.SetEnvironmentVariable("Jwt__Key", "test_secret_key_1234567890_test_secret_key");
            Environment.SetEnvironmentVariable("Jwt__Issuer", "TestIssuer");
            Environment.SetEnvironmentVariable("Jwt__Audience", "TestAudience");

            // Ensure DB is migrated before tests run
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SkillSnapContext>();
                db.Database.Migrate();
            }
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_Login_AuthorizeFlow_Works()
        {
            // Print JWT config for debugging
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");
            Console.WriteLine($"JWT config in test: Key={jwtKey}, Issuer={jwtIssuer}, Audience={jwtAudience}");

            // Register user with unique email to avoid conflicts
            var uniqueEmail = $"testuser_{Guid.NewGuid()}@example.com";
            var registerDto = new RegisterDto { Email = uniqueEmail, Password = "Test123!" };
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            var registerContent = await registerResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Register response: {registerContent}");
            Assert.True(registerResponse.IsSuccessStatusCode, $"Register failed: {registerResponse.StatusCode} - {registerContent}");

            // Login user
            var loginDto = new LoginDto { Email = uniqueEmail, Password = "Test123!" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Login response: {loginContent}");
            Assert.True(loginResponse.IsSuccessStatusCode, $"Login failed: {loginResponse.StatusCode} - {loginContent}");

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResult>();
            Assert.False(string.IsNullOrEmpty(loginResult?.Token));

            // Attach JWT to Authorization header
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            // Ensure a PortfolioUser exists for PortfolioUserId = 1
            using (var scope = _clientFactory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SkillSnapContext>();
                if (!await db.PortfolioUsers.AnyAsync(u => u.Id == 1))
                {
                    db.PortfolioUsers.Add(new SkillSnap.Api.Models.PortfolioUser
                    {
                        Id = 1,
                        Name = "Test User"
                    });
                    db.SaveChanges();
                }
            }

            // Print all routes for debugging
            Console.WriteLine("Available API routes:");
            var endpoints = _clientFactory.Services.GetService<Microsoft.AspNetCore.Routing.EndpointDataSource>();
            if (endpoints != null)
            {
                foreach (var ep in endpoints.Endpoints)
                {
                    Console.WriteLine(ep.DisplayName);
                }
            }

            // Print SkillsController POST endpoint info for debugging
            Console.WriteLine("Check SkillsController.cs for:");
            Console.WriteLine("- [ApiController]");
            Console.WriteLine("- [Route(\"api/[controller]\")] on the controller");
            Console.WriteLine("- [HttpPost] public ActionResult<Skill> Add(Skill skill) or similar signature");
            Console.WriteLine("- The Skill model matches the posted object");

            // Call [Authorize] route with token (should succeed or return 200/201/204)
            var postSkill = new { Name = "IntegrationTestSkill", Level = "Test", PortfolioUserId = 1 };
            var authResponse = await _client.PostAsJsonAsync("/api/skills", postSkill);
            var authContent = await authResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Authorized POST /api/skills response: {authContent} (Status: {authResponse.StatusCode})");
            if (!authResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Check that:");
                Console.WriteLine("- The SkillsController [Route] is [Route(\"api/[controller]\")]");
                Console.WriteLine("- The [HttpPost] method is public and not missing any required fields");
                Console.WriteLine("- The model binder matches the posted object");
                Console.WriteLine("- The database contains a PortfolioUser with Id=1");
                Console.WriteLine("- The Skills table and foreign keys are set up correctly");
            }
            Assert.True(authResponse.IsSuccessStatusCode, $"Authorized POST failed: {authResponse.StatusCode} - {authContent}");

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
