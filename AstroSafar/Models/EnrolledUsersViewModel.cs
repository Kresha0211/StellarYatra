namespace AstroSafar.Models
{
    public class EnrolledUsersViewModel
    {
        public List<Enrollment> enrollments { get; set; }
        public List<SecondaryEnroll> SecondaryEnrolls { get; set; }
        public List<HigherSecondaryEnroll> HigherSecondaryEnrolls { get; set; }
        public string categoryFilter { get; set; }

    }
}
