﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class HigherSecondaryEnroll
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("CourseAdmin")]
        public int CourseId { get; set; }

        public virtual CourseAdmin CourseAdmin { get; set; }

        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        [StringLength(150)]
        public string Address { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Please select your gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter your grade (standard).")]
        [Range(11, 12, ErrorMessage = "Standard must be  11 and 12.")]
        public int Standard { get; set; }
    }
}
