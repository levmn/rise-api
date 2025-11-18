using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseApi.Data;
using RiseApi.DTOs;
using RiseApi.Models;

namespace RiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkExperienceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkExperienceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(WorkExperienceCreateDto dto)
        {
            if (!await _context.Resumes.AnyAsync(r => r.Id == dto.ResumeId))
                return BadRequest("Resume não existe.");

            var work = new WorkExperience
            {
                Company = dto.Company,
                Title = dto.Title,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ResumeId = dto.ResumeId
            };

            _context.WorkExperiences.Add(work);
            await _context.SaveChangesAsync();

            return Ok(work);
        }
    }
}
