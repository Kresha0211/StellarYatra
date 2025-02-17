using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class AdminLogin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
