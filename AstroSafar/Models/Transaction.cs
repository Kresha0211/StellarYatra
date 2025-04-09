using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstroSafar.Models
{
    public class Transaction
    {

        [Key]
        public int Id { get; set; }

        public string RazorpayOrderId { get; set; }
        public string RazorpayPaymentId { get; set; }
        public string RazorpaySignature { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }

        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentId { get; set; }
        public virtual Enrollment Enrollment { get; set; }  // Foreign Key (if applicable)
        public DateTime PaymentDate { get; set; }

        public string Status { get; set; }  // Pending, Success, Failed
    }
}