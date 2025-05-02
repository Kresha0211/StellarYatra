using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class ExamResult
    {
        [Key]
        public int Id { get; set; }

        public int? EnrollmentId { get; set; }
        [ForeignKey("EnrollmentId")]
        public virtual Enrollment? Enrollment { get; set; }

        // Foreign Key for SecondaryEnroll
        public int? SecondaryEnrollId { get; set; }
        [ForeignKey("SecondaryEnrollId")]
        public virtual SecondaryEnroll? SecondaryEnroll { get; set; }

        // Foreign Key for HigherSecondaryEnroll
        public int? HigherSecondaryEnrollId { get; set; }
        [ForeignKey("HigherSecondaryEnrollId")]
        public virtual HigherSecondaryEnroll? HigherSecondaryEnroll { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CompletedAt { get; set; }

        // Optional fields
        public int Score { get; set; }

        public TimeSpan TimeTaken { get; set; }
    }
}