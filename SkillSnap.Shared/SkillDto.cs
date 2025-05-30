using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Shared
{
    public record SkillDto
    {
        public int Id { get; init; }

        [Required]
        [StringLength(50)]
        public string? Name { get; init; }

        [StringLength(50)]
        public string? Level { get; init; }

        public int PortfolioUserId { get; init; }
    }
}
