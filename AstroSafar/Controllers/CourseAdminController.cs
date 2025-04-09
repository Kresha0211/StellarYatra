using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroSafar.Controllers
{
    public class CourseAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly SpaceLearningDBContext _context;

        public CourseAdminController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        public IActionResult CoursesByCategory(int categoryId)
        {
            var courses = _context.courseAdmins.Where(c => c.CategoryId == categoryId).ToList();
            return View(courses);
        }

    }
}
