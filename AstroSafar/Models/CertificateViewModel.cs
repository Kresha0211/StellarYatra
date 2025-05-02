namespace AstroSafar.Models
{
    public class CertificateViewModel
    {
        public int Score { get; set; }
        public int EnrollmentId { get; set; }
        public string CourseType { get; set; }
        public string EnrollmentType { get; set; } = "primary";

    }
}