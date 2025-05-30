using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSnap.Api.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Level { get; set; }

        public int? PortfolioUserId { get; set; } // Make nullable
        public int? ProjectId { get; set; }       // Make nullable

        [ForeignKey("PortfolioUserId")]
        public PortfolioUser? User { get; set; }
    }
}
