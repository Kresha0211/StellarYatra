namespace AstroSafar.Models
{
    public class QuestionViewModel
    {
        public string Text { get; set; }
        public string Type { get; set; } // "MCQ" or "TrueFalse"
        public List<string> Options { get; set; }
    }
}
