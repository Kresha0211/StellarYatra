using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Certificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentId { get; set; }

        public virtual Enrollment Enrollment { get; set; }

        [ForeignKey("SecondaryEnroll")]
        public int? SecondaryEnrollId { get; set; }
        public virtual SecondaryEnroll SecondaryEnroll { get; set; }

        [ForeignKey("HigherSecondaryEnroll")]
        public int? HigherSecondaryEnrollId { get; set; }
        public virtual HigherSecondaryEnroll HigherSecondaryEnroll { get; set; }
        [Required]
        public DateTime IssuedOn { get; set; } = DateTime.Now;

        public string CertificateNumber { get; set; } // Optional: e.g., ASTRO-2025-0012

        public bool IsDownloaded { get; set; } = false;
    }
}