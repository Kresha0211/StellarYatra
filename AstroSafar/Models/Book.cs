using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        // Navigation
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public ICollection<BookOrderDetail> BookOrderDetails { get; set; }
    }

}