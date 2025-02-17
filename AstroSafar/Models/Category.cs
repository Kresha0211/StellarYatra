using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Primary, Secondary, Higher Secondary

        public virtual ICollection<CourseAdmin> Courses { get; set; } = new List<CourseAdmin>();

    }
}
