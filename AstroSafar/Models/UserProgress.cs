namespace AstroSafar.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int CourseId { get; set; }
        public int UnitId { get; set; }
        public bool IsCompleted { get; set; }
        public int ProgressPercentage { get; set; }
    }
}
