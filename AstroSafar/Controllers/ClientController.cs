using AstroSafar.Migrations;
using AstroSafar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AstroSafar.Controllers
{
   // [Authorize]
    public class ClientController : Controller
    {
        private readonly SpaceLearningDBContext _context;

        public ClientController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        // Show Primary courses
        public IActionResult Primary()
        {
            var primaryCourses = _context.courseAdmins
                                         .Where(c => c.Category.Name == "Primary")
                                         .ToList();
            return View(primaryCourses);
        }

        // Show Secondary courses
        public IActionResult Secondary()
        {
            var secondaryCourses = _context.courseAdmins
                                           .Where(c => c.Category.Name == "Secondary")
                                           .ToList();
            return View(secondaryCourses);
        }

        // Show Higher Secondary courses
        public IActionResult HigherSecondary()
        {
            var higherSecondaryCourses = _context.courseAdmins
                                                  .Where(c => c.Category.Name == "Higher Secondary")
                                                  .ToList();
            return View(higherSecondaryCourses);
        }

        // Show units based on selected course
        public IActionResult Units(int courseId)
        {
            var units = _context.unitAdmins
                                .Where(u => u.CourseId == courseId)
                                .ToList();
            return View(units);
        }
        [HttpGet]
        public IActionResult Enroll(int courseId)
        {
            var course = _context.courseAdmins.Find(courseId);

            if (course == null)
            {
                return NotFound();
            }

            var model = new Enrollment
            {
                CourseAdmin = course,
                CourseId = course.Id
            };

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Enroll(Enrollment model)
        //{


        //    _context.enrollments.Add(model);
        //    _context.SaveChanges();
        //    return RedirectToAction("Units", new { courseId = model.CourseId });

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Enroll(Enrollment model)
        {
            // Check if the user is already enrolled in the same course
            var existingEnrollment = _context.enrollments
                .FirstOrDefault(e => e.Email == model.Email && e.CourseId == model.CourseId);

            if (existingEnrollment != null)
            {
                // User is already enrolled, redirect to the Units page
                return RedirectToAction("Units", new { courseId = model.CourseId });
            }

            // User is not enrolled yet, proceed with enrollment
            _context.enrollments.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Units", new { courseId = model.CourseId });
        }
        [HttpGet]
        public IActionResult SecondaryEnroll(int courseId)
        {
            var course = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId);
            if (course == null) return NotFound();

            var model = new Models.SecondaryEnroll
            {
                CourseId = courseId,
                CourseAdmin = course
            };

            ViewData["Title"] = "Enroll in Secondary Course";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SecondaryEnroll(Models.SecondaryEnroll model)
        {
            _context.secondaryEnrolls.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Units", new { courseId = model.CourseId });
        }

       

        [HttpGet]
        public IActionResult HigherSecondaryEnroll(int courseId)
        {
            var course = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId); // Adjust if using CourseAdmin or another entity

            var model = new AstroSafar.Models.HigherSecondaryEnroll
            {
                CourseId = courseId,
                CourseAdmin = course // Initialize CourseAdmin to avoid null reference
            };

            ViewData["Title"] = "Enroll in Course";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HigherSecondaryEnroll(AstroSafar.Models.HigherSecondaryEnroll model)
        {
           
            _context.higherSecondaryEnrolls.Add(model);  // Add the model to the database
            _context.SaveChanges();               
            return RedirectToAction("Units", new { courseId = model.CourseId });
        }




       


    }
}
