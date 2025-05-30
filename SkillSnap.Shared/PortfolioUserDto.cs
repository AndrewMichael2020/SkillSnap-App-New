using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Shared
{
    public record PortfolioUserDto
    {
        public int Id { get; init; }

        [Required]
        [StringLength(100)]
        public string? Name { get; init; }

        [StringLength(500)]
        public string? Bio { get; init; }

        [Url]
        public string? ProfileImageUrl { get; init; }
    }
}
