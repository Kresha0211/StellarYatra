namespace AstroSafar.Models
{
    public class SubmitExamViewModel
    {
        public int CourseId { get; set; }
        public int TimeRemaining { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }
}
