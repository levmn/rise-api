using System.ComponentModel.DataAnnotations;

namespace RiseApi.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string School { get; set; } = "";

        [Required]
        [MaxLength(200)]
        public string Degree { get; set; } = "";

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Guid ResumeId { get; set; }

        public Resume? Resume { get; set; }
    }
}
