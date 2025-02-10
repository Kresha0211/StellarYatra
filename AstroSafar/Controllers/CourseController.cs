using AstroSafar.Data;
using AstroSafar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AstroSafar.Controllers
{
    public class CourseController : Controller
    {

        private readonly SpaceLearningDBContext _context;
       
        public CourseController(SpaceLearningDBContext context, ILogger<CourseController> logger)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = CourseRepository.GetInitialCourses();
            return View(courses);
        }



        public IActionResult Course()
        {
            var courses = CourseRepository.GetInitialCourses();
            return View(courses);
        }

        [HttpGet]
        public IActionResult GetMoreCourses()
        {
            // Check if the user is logged in via session
            if (HttpContext.Session.GetInt32("RegisterId") == null)
            {
                // For an AJAX call, return 401 Unauthorized so the client can handle redirection
                Response.StatusCode = 401;
                return Json(new { error = "User is not authenticated." });
            }

            // If the user is authenticated, return the additional courses as JSON
            var moreCourses = CourseRepository.GetMoreCourses();
            return Json(moreCourses);
        }
    }
}

<<<<<<< HEAD





        
       


=======
    
>>>>>>> 9f11d452a012207b723c953258bd01175e53f43d


