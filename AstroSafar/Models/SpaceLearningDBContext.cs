using Microsoft.EntityFrameworkCore;

namespace AstroSafar.Models
{
    public class SpaceLearningDBContext : DbContext
    {
        public SpaceLearningDBContext(DbContextOptions<SpaceLearningDBContext> options)
    : base(options)
        {

        }

        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Course> Courses { get; set; }
   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().ToTable("Registrations"); // Ensure correct mapping
        }

        public DbSet<Feedback> Feedbacks { get; set; }

    }

}

