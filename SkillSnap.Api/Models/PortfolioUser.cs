using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSnap.Api.Models
{
    public class PortfolioUser
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }

        [InverseProperty("User")]
        public List<Project> Projects { get; set; } = new();

        [InverseProperty("User")]
        public List<Skill> Skills { get; set; } = new();
    }
}
