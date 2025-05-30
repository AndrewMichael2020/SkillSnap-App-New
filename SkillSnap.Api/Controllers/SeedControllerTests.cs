using Xunit;
using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Controllers;
using SkillSnap.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace SkillSnap.Api.Controllers
{
    public class SeedControllerTests
    {
        private SkillSnapContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SkillSnapContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new SkillSnapContext(options);
        }

        [Fact]
        public void Seed_AddsUserProjectsAndSkills_WhenDatabaseIsEmpty()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new SeedController(context);

            // Act
            var result = controller.Seed();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Single(context.PortfolioUsers);
            Assert.Equal(20, context.Projects.Count());
            Assert.Equal(15, context.Skills.Count());
        }

        [Fact]
        public void Seed_ReturnsBadRequest_WhenAlreadySeeded()
        {
            // Arrange
            using var context = GetInMemoryContext();
            context.PortfolioUsers.Add(new Models.PortfolioUser { Name = "Test" });
            context.SaveChanges();
            var controller = new SeedController(context);

            // Act
            var result = controller.Seed();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
