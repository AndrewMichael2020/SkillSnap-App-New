using Microsoft.AspNetCore.Mvc;
using SkillSnap.Api.Data;
using SkillSnap.Api.Models;

namespace SkillSnap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly SkillSnapContext _context;

        public SeedController(SkillSnapContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Seed()
        {
            if (_context.PortfolioUsers.Any())
                return BadRequest("Database already seeded.");

            var user = new PortfolioUser
            {
                Name = "Sir Laughsalot",
                Bio = "Professional unicorn wrangler and part-time intergalactic pizza delivery expert.",
                ProfileImageUrl = "https://placekitten.com/400/400",
                Projects = new List<Project>
                {
                    new Project { Title = "Invisibility Cloak for Goldfish", Description = "Making goldfish disappear (from sight, not the bowl).", ImageUrl = "https://placekitten.com/401/200" },
                    new Project { Title = "Banana-Powered Spaceship", Description = "First fruit-fueled journey to the moon. Slippery landings guaranteed.", ImageUrl = "https://placekitten.com/402/200" },
                    new Project { Title = "Self-Tying Shoelaces for Centipedes", Description = "Automating 100+ shoes at once. Centipedes rejoice!", ImageUrl = "https://placekitten.com/403/200" },
                    new Project { Title = "Time-Traveling Toaster", Description = "Burnt toast from the future, today.", ImageUrl = "https://placekitten.com/404/200" },
                    new Project { Title = "Singing Refrigerator", Description = "Keeps your food cool and your spirits cooler.", ImageUrl = "https://placekitten.com/405/200" },
                    new Project { Title = "Unicorn Traffic Control", Description = "Managing magical traffic jams in fairyland.", ImageUrl = "https://placekitten.com/406/200" },
                    new Project { Title = "Cloud-Shaped Pillow Factory", Description = "For the fluffiest dreams imaginable.", ImageUrl = "https://placekitten.com/407/200" },
                    new Project { Title = "Reverse Microwave", Description = "Turns hot soup into ice cream in seconds.", ImageUrl = "https://placekitten.com/408/200" },
                    new Project { Title = "Hamster-Powered Generator", Description = "Renewable energy, one wheel at a time.", ImageUrl = "https://placekitten.com/409/200" },
                    new Project { Title = "Teleporting Rubber Duck", Description = "Never lose your bath toy again.", ImageUrl = "https://placekitten.com/410/200" },
                    new Project { Title = "Quantum Pancake Flipper", Description = "Flips pancakes in all universes simultaneously.", ImageUrl = "https://placekitten.com/411/200" },
                    new Project { Title = "WiFi-Enabled Toothbrush", Description = "Tweets every brush stroke. #DentalHygiene", ImageUrl = "https://placekitten.com/412/200" },
                    new Project { Title = "Solar-Powered Night Light", Description = "Charges during the day, shines at night. Wait...", ImageUrl = "https://placekitten.com/413/200" },
                    new Project { Title = "AI Cat Translator", Description = "Finally, your cat's meows decoded: 'Feed me.'", ImageUrl = "https://placekitten.com/414/200" },
                    new Project { Title = "Origami Folding Robot", Description = "Folds paper cranes and existential questions.", ImageUrl = "https://placekitten.com/415/200" },
                    new Project { Title = "Underwater Hair Dryer", Description = "For mermaids with style.", ImageUrl = "https://placekitten.com/416/200" },
                    new Project { Title = "Glow-in-the-Dark Sunglasses", Description = "For seeing in the dark... sort of.", ImageUrl = "https://placekitten.com/417/200" },
                    new Project { Title = "Edible Socks", Description = "For the truly hungry traveler.", ImageUrl = "https://placekitten.com/418/200" },
                    new Project { Title = "Rocket-Powered Skateboard", Description = "For when you need to get to school yesterday.", ImageUrl = "https://placekitten.com/419/200" },
                    new Project { Title = "Mood-Sensing Umbrella", Description = "Changes color based on your feelings. Or the weather. Or randomly.", ImageUrl = "https://placekitten.com/420/200" }
                },
                Skills = new List<Skill>
                {
                    new Skill { Name = "Expert Unicorn Wrangler", Level = "Legendary" },
                    new Skill { Name = "Banana Propulsion Engineering", Level = "Advanced" },
                    new Skill { Name = "Centipede Shoelace Management", Level = "Intermediate" },
                    new Skill { Name = "Quantum Pancake Flipping", Level = "Master" },
                    new Skill { Name = "Hamster Motivation", Level = "Guru" },
                    new Skill { Name = "Toaster Time Travel", Level = "Beginner (stuck in 1985)" },
                    new Skill { Name = "Rubber Duck Teleportation", Level = "Occasionally Successful" },
                    new Skill { Name = "Refrigerator Karaoke", Level = "Tone-Deaf" },
                    new Skill { Name = "Origami Philosophy", Level = "Paper Sage" },
                    new Skill { Name = "Mermaid Hair Styling", Level = "Splashy" },
                    new Skill { Name = "Glow-in-the-Dark Sunglass Design", Level = "Blinding" },
                    new Skill { Name = "Edible Sock Baking", Level = "Chewy" },
                    new Skill { Name = "Rocket Skateboard Piloting", Level = "Speedy (and bruised)" },
                    new Skill { Name = "Mood Umbrella Interpretation", Level = "Moody" },
                    new Skill { Name = "Cat Meow Translation", Level = "Fluent in Feline" }
                }
            };

            _context.PortfolioUsers.Add(user);
            _context.SaveChanges();

            return Ok("Database seeded with one user, 20 projects, and 15 skills.");
        }
    }
}
