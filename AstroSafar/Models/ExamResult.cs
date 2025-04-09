using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class ExamResult
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentId { get; set; }

        public virtual Enrollment Enrollment { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CompletedAt { get; set; }

        // Optional fields
        public int Score { get; set; }

        public TimeSpan TimeTaken { get; set; }
    }
}