using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class UnitAdmin
    {
        //    [Key]
        //    public int Id { get; set; } // Primary Key
        //    public string Name { get; set; }
        //    public string VideoUrl { get; set; } // YouTube Video Link

        //    [ForeignKey("CourseAdmin")]
        //    public int CourseId { get; set; } // Foreign Key
        //    public CourseAdmin Courses { get; set; } // Navigation Property
        //}
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public string Name { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid video URL.")]
        public string VideoUrl { get; set; } // YouTube Video Link

        [ForeignKey("CourseAdmin")]
        public int CourseId { get; set; } // Foreign Key

        // Navigation Property
        public virtual CourseAdmin CourseAdmin { get; set; }
    }
}