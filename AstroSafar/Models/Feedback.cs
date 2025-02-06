using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; } // Feedback ID (Primary Key)

        //[Required]
       // public string UserId { get; set; } // ID of the user giving feedback

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }
    }
}

