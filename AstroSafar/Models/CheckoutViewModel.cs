using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // E.g., "UPI", "CreditCard", etc.

        // ← new property to hold your cart items
        public List<CartItemViewModel> CartItems { get; set; } = new();

        // ← your existing total; we'll set this in the controller
        public decimal TotalAmount { get; set; }
        public string PaymentId { get; set; }
    }


}