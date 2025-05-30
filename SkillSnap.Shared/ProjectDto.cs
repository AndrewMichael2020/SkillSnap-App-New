using System.ComponentModel.DataAnnotations;

namespace SkillSnap.Shared
{
    public record ProjectDto
    {
        public int Id { get; init; }

        [Required]
        [StringLength(100)]
        public string? Title { get; init; }

        [StringLength(1000)]
        public string? Description { get; init; }

        [Url]
        public string? ImageUrl { get; init; }

        public int PortfolioUserId { get; init; }
    }
}
