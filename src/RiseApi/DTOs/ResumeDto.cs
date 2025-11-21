namespace RiseApi.DTOs
{
    public class ResumeCreateDto
    {
        public string Goal { get; set; } = "";
        public Guid UserId { get; set; }
    }

    public class ResumeDto
    {
        public Guid Id { get; set; }
        public string Goal { get; set; } = "";
        public Guid UserId { get; set; }
    }
}
