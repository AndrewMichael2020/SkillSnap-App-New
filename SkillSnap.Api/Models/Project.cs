using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSnap.Api.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public int? PortfolioUserId { get; set; } // Make nullable

        [ForeignKey("PortfolioUserId")]
        public PortfolioUser? User { get; set; }

        public List<Skill> Skills { get; set; } = new();
    }
}
