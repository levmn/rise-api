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
            var resume = await _context.Resumes.FindAsync(dto.ResumeId);
            if (resume == null)
                return NotFound("Currículo não encontrado.");

            var education = new Education
            {
                School = dto.School,
                Degree = dto.Degree,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ResumeId = dto.ResumeId
            };

            _context.Education.Add(education);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), new { id = education.Id }, new
            {
                data = new EducationDto
                {
                    Id = education.Id,
                    School = education.School,
                    Degree = education.Degree,
                    StartDate = education.StartDate,
                    EndDate = education.EndDate,
                    ResumeId = education.ResumeId
                },
                links = new
                {
                    resume = Url.Action("GetById", "Resume", new { id = dto.ResumeId }),
                    delete = Url.Action(nameof(Delete), new { id = education.Id })
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var education = await _context.Education.FindAsync(id);
            if (education == null) return NotFound();

            _context.Education.Remove(education);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
