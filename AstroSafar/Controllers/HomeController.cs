using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AstroSafar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CourseGuideline()
        {
            return View();
        }

        //public IActionResult CourseGuideline()
        //{
        //    return View();
        //}

        //public IActionResult CourseGuideline( string Title , String Desciption)
        //{
        //    // ✅ Pass Course Guidelines to Index.cshtml
        //    var guidelines = new List<CourseGuildeline> // ✅ Fixed typo (CourseGuildeline → CourseGuideline)
        //    {
        //        new CourseGuildeline { Title = "Welcome to Space Learning!", Description = "Explore the wonders of the universe with our structured courses." },
        //        new CourseGuildeline { Title = "Course Structure", Description = "The course is divided into modules covering planets, stars, galaxies, and more." },
        //        new CourseGuildeline { Title = "Interactive Learning", Description = "Engage with quizzes, videos, and interactive simulations." },
        //        new CourseGuildeline { Title = "Assessment & Certification", Description = "Complete assessments to earn certificates for each module." },
        //        new CourseGuildeline { Title = "Support & Community", Description = "Join our discussion forums and get support from experts." }
        //    };

        //    return View(guidelines); // ✅ Pass data to Index.cshtml
        //}



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

