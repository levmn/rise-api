using System.ComponentModel.DataAnnotations;

namespace RiseApi.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        public ICollection<Resume>? Resumes { get; set; }
    }
}
