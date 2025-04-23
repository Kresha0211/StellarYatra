using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class BookOrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [ForeignKey("BookOrder")]
        public int BookOrderId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Navigation
        public BookOrder BookOrder { get; set; }
        public Book Book { get; set; }
    }

}