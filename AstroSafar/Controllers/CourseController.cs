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

        public CourseController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        //[Authorize] // Ensures only authenticated users can access
        //public IActionResult MoreCourses()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (userId != null)
        //    {
        //        //var courses = _context.Courses.ToList();
        //        //return View(courses);
        //        var moreCourses = CourseRepository.GetMoreCourses();
        //        return View("Course", moreCourses);c
        //    }

        //    return RedirectToAction("Login", "Account");
        //}
        public IActionResult Course()
        {
            var courses = CourseRepository.GetInitialCourses();
            return View(courses);
        }
        public IActionResult ShowMore()
        {
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine("✅ User is logged in, redirecting to MoreCourses...");
                return RedirectToAction("MoreCourses");
            }
            else
            {
                Console.WriteLine("❌ User is NOT logged in, redirecting to Login...");
                return RedirectToAction("Login", "Account");
            }
        }

        [Authorize] // Ensures only logged-in users can access
        public IActionResult MoreCourses()
        {
            // 🔹 Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }

            var moreCourses = CourseRepository.GetMoreCourses();
            return View(moreCourses); // Ensure "Index.cshtml" exists inside Views/Course/
        }


        
        // Only logged-in users can access this
        //public IActionResult MoreCourses()
        //{
        //    var moreCourses = CourseRepository.GetMoreCourses();
        //    return View("Course", moreCourses); // Reusing Index.cshtml for more courses
        //}
        public IActionResult Index()
        {
            return View();
        }


    }
}
