namespace RiseApi.DTOs
{
    public class ResumeCreateDto
    {
        public string Goal { get; set; } = "";
        public Guid UserId { get; set; }
    }

    public class ResumeResponseDto
    {
        public Guid Id { get; set; }
        public string Goal { get; set; } = "";
        public Guid UserId { get; set; }
    }
}
