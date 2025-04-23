using AstroSafar.Hubs;
using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AstroSafar.Controllers
{
    public class SubmitFeedbackController : Controller
    {
        private readonly SpaceLearningDBContext _context;
        private readonly IHubContext<ChatHub> _hubContext;


        public SubmitFeedbackController(SpaceLearningDBContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;

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
