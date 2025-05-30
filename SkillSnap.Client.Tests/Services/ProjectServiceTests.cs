using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using SkillSnap.Client.Services;
using Project = SkillSnap.Client.Services.ProjectService.Project;

namespace SkillSnap.Client.Tests.Services
{
    public class ProjectServiceTests
    {
        private ProjectService CreateService(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            return new ProjectService(httpClient);
        }

        [Fact]
        public async Task GetProjectsAsync_ReturnsProjects_WhenApiReturnsData()
        {
            var projects = new List<Project>
            {
                new Project { Id = 1, Name = "Test", Description = "Desc" }
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(projects)
            };
            var service = CreateService(response);

            var result = await service.GetProjectsAsync();

            Assert.Single(result);
            Assert.Equal("Test", result[0].Name);
        }

        [Fact]
        public async Task GetProjectsAsync_ReturnsEmptyList_OnApiFailure()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var service = CreateService(response);

            var result = await service.GetProjectsAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProjectsAsync_ReturnsEmptyList_OnException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var service = new ProjectService(httpClient);

            var result = await service.GetProjectsAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task AddProjectAsync_SuccessfulResponse_DoesNotThrow()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            var service = CreateService(response);

            var project = new Project { Name = "Test", Description = "Desc" };

            var ex = await Record.ExceptionAsync(() => service.AddProjectAsync(project));
            Assert.Null(ex);
        }

        [Fact]
        public async Task AddProjectAsync_UnsuccessfulResponse_ThrowsHttpRequestException()
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request")
            };
            var service = CreateService(response);

            var project = new Project { Name = "Test", Description = "Desc" };

            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => service.AddProjectAsync(project));
            Assert.Contains("Failed to add project", ex.Message);
        }

        [Fact]
        public async Task AddProjectAsync_ThrowsException_OnNetworkError()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var service = new ProjectService(httpClient);

            var project = new Project { Name = "Test", Description = "Desc" };

            await Assert.ThrowsAsync<HttpRequestException>(() => service.AddProjectAsync(project));
        }

        [Fact]
        public async Task AddProjectAsync_SendsCorrectRequest()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            HttpRequestMessage? capturedRequest = null;

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, ct) => capturedRequest = req)
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created));

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var service = new ProjectService(httpClient);

            var project = new Project { Name = "Test", Description = "Desc" };
            await service.AddProjectAsync(project);

            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Contains("/api/projects", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetProjectsAsync_UsesCorrectEndpoint()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            HttpRequestMessage? capturedRequest = null;

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, ct) => capturedRequest = req)
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(new List<Project>())
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var service = new ProjectService(httpClient);

            await service.GetProjectsAsync();

            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Get, capturedRequest.Method);
            Assert.Contains("/api/projects", capturedRequest.RequestUri.ToString());
        }
    }
}
