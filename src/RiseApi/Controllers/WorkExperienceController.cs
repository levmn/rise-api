using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiseApi.Data;
using RiseApi.DTOs;
using RiseApi.Models;

namespace RiseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            var resume = await _context.Resumes.FindAsync(dto.ResumeId);
            if (resume == null)
                return NotFound("Currículo não encontrado.");

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

            return CreatedAtAction(nameof(Create), new { id = work.Id }, new
            {
                data = new WorkExperienceDto
                {
                    Id = work.Id,
                    Company = work.Company,
                    Title = work.Title,
                    StartDate = work.StartDate,
                    EndDate = work.EndDate,
                    ResumeId = work.ResumeId
                },
                links = new
                {
                    resume = Url.Action("GetById", "Resume", new { id = dto.ResumeId }),
                    delete = Url.Action(nameof(Delete), new { id = work.Id })
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var work = await _context.WorkExperiences.FindAsync(id);
            if (work == null) return NotFound();

            _context.WorkExperiences.Remove(work);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
