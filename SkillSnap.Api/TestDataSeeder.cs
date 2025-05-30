using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SkillSnap.Api.Models;
using SkillSnap.Api.Data;

public static class TestDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<SkillSnapContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        try
        {
            // Remove all users for repeatable seeding (dev only)
            foreach (var user in context.Users.ToList())
            {
                await userManager.DeleteAsync(user);
            }
            context.SaveChanges();

            // Seed users
            if (!context.Users.Any())
            {
                var testUsers = new List<ApplicationUser>
                {
                    new ApplicationUser { UserName = "funnycat", Email = "cat@meow.com" },
                    new ApplicationUser { UserName = "sillydog", Email = "dog@woof.com" },
                    new ApplicationUser { UserName = "jokebird", Email = "bird@tweet.com" },
                    new ApplicationUser { UserName = "punnybunny", Email = "bunny@hop.com" },
                    new ApplicationUser { UserName = "quirkysheep", Email = "sheep@baa.com" },
                    new ApplicationUser { UserName = "laughinglion", Email = "lion@roar.com" },
                    new ApplicationUser { UserName = "gigglefish", Email = "fish@blub.com" },
                    new ApplicationUser { UserName = "snickerhorse", Email = "horse@neigh.com" },
                    new ApplicationUser { UserName = "comicmonkey", Email = "monkey@ooh.com" },
                    new ApplicationUser { UserName = "prankypanda", Email = "panda@bamboo.com" },
                    new ApplicationUser { UserName = "wittywalrus", Email = "walrus@tusk.com" },
                    new ApplicationUser { UserName = "banterbear", Email = "bear@growl.com" },
                    new ApplicationUser { UserName = "jestergiraffe", Email = "giraffe@neck.com" },
                    new ApplicationUser { UserName = "humorowl", Email = "owl@hoot.com" },
                    new ApplicationUser { UserName = "smirkysnake", Email = "snake@hiss.com" },
                    new ApplicationUser { UserName = "chucklecheetah", Email = "cheetah@fast.com" },
                    new ApplicationUser { UserName = "grinnygoat", Email = "goat@bleat.com" },
                    new ApplicationUser { UserName = "teasertiger", Email = "tiger@stripe.com" },
                    new ApplicationUser { UserName = "mockingmoose", Email = "moose@antler.com" },
                    new ApplicationUser { UserName = "sassyseal", Email = "seal@bark.com" }
                };

                foreach (var user in testUsers)
                {
                    var result = await userManager.CreateAsync(user, "Test123!@#");
                    if (!result.Succeeded)
                    {
                        // Log errors for debugging
                        Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding error: {ex.Message}");
            throw;
        }

        // Comment out or remove these blocks to avoid FK errors:
        // Seed skills
        if (!context.Set<Skill>().Any())
        {
            var testSkills = new List<Skill>
            {
                new Skill { Name = "Juggling Chainsaws", Level = "Funny" },
                new Skill { Name = "Extreme Napping", Level = "Expert" },
                new Skill { Name = "Speed Typing with Toes", Level = "Intermediate" },
                new Skill { Name = "Sarcastic Commenting", Level = "Advanced" },
                new Skill { Name = "Unicorn Taming", Level = "Legendary" },
                new Skill { Name = "Invisible Ink Writing", Level = "Beginner" },
                new Skill { Name = "Competitive Yawning", Level = "Champion" },
                new Skill { Name = "Origami with Pizza Boxes", Level = "Creative" },
                new Skill { Name = "Underwater Basket Weaving", Level = "Wet" },
                new Skill { Name = "Professional Meme Creation", Level = "Dank" }
            };
            context.Set<Skill>().AddRange(testSkills);
            context.SaveChanges();
        }

        // Seed projects
        if (!context.Set<Project>().Any())
        {
            var testProjects = new List<Project>
            {
                new Project { Name = "Build a Cat-Powered Car", Description = "Harnessing feline energy for eco-friendly travel." },
                new Project { Name = "The Laughing Library", Description = "A collection of the world's funniest books." },
                new Project { Name = "Banana Phone App", Description = "Turning real bananas into working phones." },
                new Project { Name = "Invisible Art Gallery", Description = "Come see what you can't see!" },
                new Project { Name = "World's Largest Rubber Duck", Description = "A quacking good time for all ages." },
                new Project { Name = "AI That Writes Dad Jokes", Description = "Because the world needs more groans." },
                new Project { Name = "Teleporting Toast", Description = "Breakfast delivered instantly." },
                new Project { Name = "Dancing Robot Parade", Description = "Robots with all the right moves." },
                new Project { Name = "Self-Watering Cactus", Description = "For the truly forgetful plant owner." },
                new Project { Name = "Cloud Storage (Literally)", Description = "Store your files in actual clouds." }
            };
            context.Set<Project>().AddRange(testProjects);
            context.SaveChanges();
        }
    }
}
