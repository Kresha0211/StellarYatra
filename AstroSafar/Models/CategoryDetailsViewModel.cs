namespace AstroSafar.Models
{
    public class CategoryDetailsViewModel
    {
        public int CategoryId { get; set; }
        public List<CourseAdmin> Courses { get; set; }
        public List<UnitAdmin> Units { get; set; }
    }
}
