using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroSafar.Controllers
{
    public class CategoryController : Controller
    {
        
        private readonly SpaceLearningDBContext _context;

        public CategoryController(SpaceLearningDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        
    }
}
