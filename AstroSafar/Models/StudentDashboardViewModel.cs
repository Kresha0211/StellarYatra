namespace AstroSafar.Models
{
    public class StudentDashboardViewModel
    {
        public int ProgressPercentage { get; set; } // Overall course progress
        public List<CourseProgressViewModel> CourseProgressList { get; set; } // List of course progress
    }

    public class CourseProgressViewModel
    {
        public string CourseName { get; set; }
        public int ProgressPercentage { get; set; }
    }
     // Request model for JSON parsing
        public class UpdateUnitProgressRequest
        {
            public int CourseId { get; set; }
            public int UnitId { get; set; }
        }


}

