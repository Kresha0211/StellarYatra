using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class UserCourseProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } // Student's email

        [Required]
        [ForeignKey("CourseAdmin")]
        public int CourseId { get; set; } // Foreign Key to CourseAdmin

        public int ProgressPercentage { get; set; } // Track overall course progress

        // Navigation Property (optional)
        public virtual CourseAdmin CourseAdmin { get; set; }
    }
}
