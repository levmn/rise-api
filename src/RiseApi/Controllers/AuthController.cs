using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using RiseApi.Data;
using RiseApi.DTOs.Auth;
using RiseApi.Models;
using RiseApi.Services;

namespace RiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;

        public AuthController(AppDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthDto dto)
        {
            var exists = await _context.Users
                .CountAsync(u => u.Username == dto.Username);

            if (exists > 0)
                return BadRequest("Username já existe.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuário criado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null)
                return Unauthorized("Usuário ou senha inválidos.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Usuário ou senha inválidos.");

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }
    }
}
