using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseApi.Data;
using RiseApi.DTOs;
using RiseApi.Models;

namespace RiseApi.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly LinkGenerator _linkGenerator;

        public UserController(AppDbContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(
            int page = 1,
            int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("page e pageSize devem ser maiores que 0.");

            var query = _context.Users.AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username
                })
                .ToListAsync();

            var links = new
            {
                self = Url.Action("GetAll", new { page, pageSize }),
                next = page < totalPages ? Url.Action("GetAll", new { page = page + 1, pageSize }) : null,
                prev = page > 1 ? Url.Action("GetAll", new { page = page - 1, pageSize }) : null
            };

            return Ok(new
            {
                page,
                pageSize,
                totalItems,
                totalPages,
                data = users,
                links
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var response = new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };

            var links = new
            {
                self = Url.Action(nameof(GetById), new { id }),
                delete = Url.Action(nameof(Delete), new { id })
            };

            return Ok(new { data = response, links });
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserCreateDto dto)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (userExists)
                return Conflict("Nome de usuário já está em uso.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newUser.Id },
                new
                {
                    data = new { newUser.Id, newUser.Username },
                    links = new
                    {
                        self = Url.Action(nameof(GetById), new { id = newUser.Id }),
                        all = Url.Action(nameof(GetAll))
                    }
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
