using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class UnitAdmin
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public string Name { get; set; }
        public string Content { get; set; }

        public string ImageURL { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid video URL.")]
        public string VideoUrl { get; set; } 

        [ForeignKey("CourseAdmin")]
        public int CourseId { get; set; } // Foreign Key

        // Navigation Property
        public virtual CourseAdmin CourseAdmin { get; set; }
    }
}