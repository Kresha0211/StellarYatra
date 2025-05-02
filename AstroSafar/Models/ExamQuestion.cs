    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    namespace AstroSafar.Models
    {
        public class ExamQuestion
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string QuestionText { get; set; }

            [Required]
            public string Options { get; set; } // Store options as a comma-separated string

            [Required]
            public string CorrectAnswer { get; set; }

            [Required]
            public string QuestionType { get; set; } // "MCQ", "TrueFalse"

            [ForeignKey("Course")]
            public int CourseId { get; set; }
            public CourseAdmin Course { get; set; } // Navigation property

        }
    }
