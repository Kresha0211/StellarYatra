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
        //public DbSet<Course> Courses { get; set; }

        public DbSet<AdminLogin> AdminLogins { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<CourseA> CoursesA { get; set; }
        //public DbSet<Unit> Units { get; set; }
        public DbSet<CourseAdmin> courseAdmins { get; set; }
        public DbSet<UnitAdmin> unitAdmins { get; set; }

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

    }

}

