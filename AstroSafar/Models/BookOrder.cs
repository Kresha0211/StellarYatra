using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class BookOrder
    {
        [Key]
        public int BookOrderId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign key to Registration.Id

        [ForeignKey("UserId")]
        public Registration User { get; set; } // Navigation property

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        // Delivery details
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string? PaymentId { get; set; } // Razorpay Payment ID (nullable)

        // Navigation
        public ICollection<BookOrderDetail> BookOrderDetails { get; set; }
    }

}