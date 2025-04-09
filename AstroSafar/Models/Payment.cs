using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstroSafar.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        [Required]
        [ForeignKey("Enrollment")]
        public int EnrollmentId { get; set; }
        public virtual Enrollment Enrollment { get; set; }

        [Required(ErrorMessage = "Please enter the payment amount.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please enter the transaction ID.")]
        public string TransactionId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
