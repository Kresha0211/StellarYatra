using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<AdminLogin> AdminLogins { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<CourseAdmin> courseAdmins { get; set; }
        public DbSet<UnitAdmin> unitAdmins { get; set; }
        public DbSet<Enrollment> enrollments { get; set; }
        public DbSet<SecondaryEnroll> secondaryEnrolls { get; set; }
        public DbSet<HigherSecondaryEnroll> higherSecondaryEnrolls { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>().ToTable("Registrations");
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Primary" },
               new Category { Id = 2, Name = "Secondary" },
               new Category { Id = 3, Name = "Higher Secondary" }
           );

            modelBuilder.Entity<CourseAdmin>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Feedback> Feedbacks { get; set; }
        public object CourseAdmins { get; internal set; }
    }

}

