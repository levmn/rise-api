namespace RiseApi.DTOs
{
    public class EducationCreateDto
    {
        public string School { get; set; } = "";
        public string Degree { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ResumeId { get; set; }
    }

    public class EducationUpdateDto
    {
        public string School { get; set; } = "";
        public string Degree { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class EducationDto
    {
        public int Id { get; set; }
        public string School { get; set; } = "";
        public string Degree { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ResumeId { get; set; }
    }
}
