using System.ComponentModel.DataAnnotations;

namespace RiseApi.Models
{
    public class Resume
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(1000)]
        public string Goal { get; set; } = null!;

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public User User { get; set; } = null!;

        public ICollection<Education>? EducationItems { get; set; }
        public ICollection<WorkExperience>? WorkExperiences { get; set; }
    }
}
