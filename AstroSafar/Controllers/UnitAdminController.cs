using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroSafar.Controllers
{
    public class UnitAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly SpaceLearningDBContext _context;

        public UnitAdminController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        public IActionResult UnitsByCourse(int courseId)
        {
            var units = _context.unitAdmins.Where(u => u.CourseId == courseId).ToList();
            return View(units);
        }
    }
}
