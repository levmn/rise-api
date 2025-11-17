using System.ComponentModel.DataAnnotations;

namespace RiseApi.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Company { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = "";

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ResumeId { get; set; }

        public Resume? Resume { get; set; }
    }
}
