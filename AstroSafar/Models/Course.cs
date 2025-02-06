using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        //[ForeignKey("Category")]
        //public int CategoryID { get; set; }
        //public Category Category { get; set; } // Navigation property

        [MaxLength(20)]
        public string Duration { get; set; } // Example: "4 hours"

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = false;

        [MaxLength(255)]
        public string ImageURL { get; set; } // Optional course image

        //[ForeignKey("Instructor")]
      //  public int InstructorID { get; set; }
        ////public User Instructor { get; set; } // Navigation property
    }//

}

