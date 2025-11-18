using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseApi.Data;
using RiseApi.DTOs;
using RiseApi.Models;

namespace RiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EducationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(EducationCreateDto dto)
        {
            if (!await _context.Resumes.AnyAsync(r => r.Id == dto.ResumeId))
                return BadRequest("Resume não existe.");

            var edu = new Education
            {
                School = dto.School,
                Degree = dto.Degree,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ResumeId = dto.ResumeId
            };

            _context.Education.Add(edu);
            await _context.SaveChangesAsync();

            return Ok(edu);
        }
    }
}
