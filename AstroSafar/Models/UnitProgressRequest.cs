namespace AstroSafar.Models
{
    public class UnitProgressRequest
    {
        public int CourseId { get; set; }
        public int UnitId { get; set; }
        public bool IsCompleted { get; set; }
        public int ProgressPercentage { get; set; } // Add this property

    }
}
