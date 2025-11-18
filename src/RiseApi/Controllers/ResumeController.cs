using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseApi.Data;
using RiseApi.DTOs;
using RiseApi.Models;

namespace RiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResumeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResumeResponseDto>>> GetAll()
        {
            var resumes = await _context.Resumes
                .Select(r => new ResumeResponseDto
                {
                    Id = r.Id,
                    Goal = r.Goal,
                    UserId = r.UserId
                })
                .ToListAsync();

            return Ok(resumes);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ResumeCreateDto dto)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
                return BadRequest("Usuário não existe.");

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
