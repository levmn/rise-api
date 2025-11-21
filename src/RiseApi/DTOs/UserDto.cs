namespace RiseApi.DTOs
{
    public class UserCreateDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
    }
}
