namespace AstroSafar.Models
{
    public class ExamSubmissionViewModel
    {
        public int CourseId { get; set; }
        public string CourseType { get; set; } = "Primary";

        public int EnrollmentId { get; set; }

        public List<UserAnswer> UserAnswers { get; set; }
        public int TimeRemaining { get; set; } // in seconds
    }
}