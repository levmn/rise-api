using Microsoft.EntityFrameworkCore;
using RiseApi.Models;

namespace RiseApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Resume> Resumes { get; set; } = null!;
        public DbSet<Education> Education { get; set; } = null!;
        public DbSet<WorkExperience> WorkExperiences { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Resume)
                .WithMany(r => r.EducationItems)
                .HasForeignKey(e => e.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkExperience>()
                .HasOne(w => w.Resume)
                .WithMany(r => r.WorkExperiences)
                .HasForeignKey(w => w.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
