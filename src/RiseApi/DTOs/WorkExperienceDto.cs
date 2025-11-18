namespace RiseApi.DTOs
{
    public class WorkExperienceCreateDto
    {
        public string Company { get; set; } = "";
        public string Title { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ResumeId { get; set; }
    }
}
