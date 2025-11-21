using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResumeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("page e pageSize devem ser maiores que 0.");

            var query = _context.Resumes.AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var resumes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new ResumeDto
                {
                    Id = r.Id,
                    Goal = r.Goal,
                    UserId = r.UserId
                })
                .ToListAsync();

            var links = new
            {
                self = Url.Action(nameof(GetAll), new { page, pageSize }),
                next = page < totalPages ? Url.Action(nameof(GetAll), new { page = page + 1, pageSize }) : null,
                prev = page > 1 ? Url.Action(nameof(GetAll), new { page = page - 1, pageSize }) : null
            };

            return Ok(new
            {
                page,
                pageSize,
                totalItems,
                totalPages,
                data = resumes,
                links
            });
        }


        [HttpPost]
        public async Task<ActionResult> Create(ResumeCreateDto dto)
        {
            var exists = await _context.Users
                .CountAsync(u => u.Id == dto.UserId);

            if (exists == 0)
                return BadRequest("Usuário não encontrado.");

            var resume = new Resume
            {
                Goal = dto.Goal,
                UserId = dto.UserId
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), resume);
        }
    }
}
