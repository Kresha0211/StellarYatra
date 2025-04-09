using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstroSafar.Models
{
    [Table("StudentCertificates")]
    public class StudentCertificate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnrollmentId { get; set; }

        [ForeignKey("EnrollmentId")]
        public virtual Enrollment Enrollment { get; set; }

        [Required]
        public DateTime IssuedDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsPaid { get; set; } = true;
    }
}
