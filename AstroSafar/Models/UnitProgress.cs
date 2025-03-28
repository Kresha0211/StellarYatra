using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class UnitProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; } // Student's email

        [Required]
        [ForeignKey("CourseAdmin")]
        public int CourseId { get; set; } // Foreign Key to CourseAdmin

        [Required]
        [ForeignKey("UnitAdmin")]
        public int UnitId { get; set; } // Foreign Key to UnitAdmin

        public bool IsCompleted { get; set; } = false;     // Fully completed?
        public int ProgressPercentage { get; set; } = 0;    // 0-100%
       

        // Navigation Properties
        public virtual CourseAdmin CourseAdmin { get; set; }
        public virtual UnitAdmin UnitAdmin { get; set; }
    }
}
