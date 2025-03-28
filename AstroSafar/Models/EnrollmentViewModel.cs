namespace AstroSafar.Models
{
    public class EnrollmentViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public bool IsEnrolled { get; set; } // Check if the user is already enrolled
    }
}
