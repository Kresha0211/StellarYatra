﻿using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
