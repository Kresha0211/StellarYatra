using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class CourseAdmin
    {
      
        [Key]
        public int Id { get; set; }
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Course description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Course duration is required.")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Please select a valid category.")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation Property
        public virtual Category Category { get; set; }

        // Collection of Units
        public virtual ICollection<UnitAdmin> UnitAdmins { get; set; } = new List<UnitAdmin>();
    }
}
