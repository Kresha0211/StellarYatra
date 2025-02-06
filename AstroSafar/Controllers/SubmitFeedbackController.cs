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
                _context.SaveChanges(); // ✅ Save to database

                TempData["Message"] = "Thank you! Your feedback has been submitted.";
                return RedirectToAction("SubmitFeedback"); // ✅ Redirect to avoid duplicate submissions
            }

            return View("SubmitFeedback", feedback);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
