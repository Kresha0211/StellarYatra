using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstroSafar.Models
{
    public class ExamQuestionsViewModel
    {
        public int CourseId { get; set; } // Selected course


        [ForeignKey("Course")]
        // public int CourseId { get; set; }
        public CourseAdmin Course { get; set; } // Navigation property
        public List<QuestionInputModel> Questions { get; set; } = new List<QuestionInputModel>();
    }
    public class QuestionInputModel
    {
        public int QuestionId { get; set; } // Ensure this exists

        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // "MCQ" or "TrueFalse"
        public string Options { get; set; } // Comma-separated for MCQ
        public string CorrectAnswer { get; set; }

    }

}

