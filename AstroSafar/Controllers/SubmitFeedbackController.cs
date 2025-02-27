using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroSafar.Controllers
{
    public class SubmitFeedbackController : Controller
    {
        private readonly SpaceLearningDBContext _context;

        public SubmitFeedbackController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        public IActionResult SubmitFeedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges(); 

                TempData["Message"] = "Thank you! Your Response has being Resolve Soon!";
                return RedirectToAction("SubmitFeedback");
            }

            return View("SubmitFeedback", feedback);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
