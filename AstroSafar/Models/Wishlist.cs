using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign key to Registration.Id

        [ForeignKey("UserId")]
        public Registration User { get; set; } // Navigation property


        [ForeignKey("Book")]
        public int BookId { get; set; }

        // Navigation
        public Book Book { get; set; }
    }

}