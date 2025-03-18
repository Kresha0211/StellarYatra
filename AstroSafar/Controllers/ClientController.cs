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

        // Needed

        //[HttpGet]
        //public IActionResult Enroll(int courseId)
        //{
        //    var course = _context.courseAdmins.Find(courseId);

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new Enrollment
        //    {
        //        CourseAdmin = course,
        //        CourseId = course.Id,
        //    };


        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Enroll(int courseId)
        {
            // Get current user's email
            string userEmail = HttpContext.Session.GetString("UserEmail");

            // If email is available, check for existing enrollment
            if (!string.IsNullOrEmpty(userEmail))
            {
                // Use FirstOrDefault to check if the enrollment exists
                var existingEnrollment = _context.enrollments
                    .FirstOrDefault(e => e.CourseId == courseId && e.Email == userEmail);

                // If enrollment exists, redirect to Units
                if (existingEnrollment != null)
                {
                    return RedirectToAction("Units", new { courseId });
                }
            }

            // If not enrolled, show the enrollment form
            var course = _context.courseAdmins.Find(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var model = new Enrollment
            {
                CourseAdmin = course,
                CourseId = courseId,
                Email = userEmail ?? ""
            };

            return View(model);
        }
        //needed most

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(Enrollment model)
        {
            _context.enrollments.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Units", new { courseId = model.CourseId });

        }

      

        //[HttpGet]
        //public IActionResult SecondaryEnroll(int courseId)
        //{
        //    var course = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId);
        //    if (course == null) return NotFound();

        //    var model = new Models.SecondaryEnroll
        //    {
        //        CourseId = courseId,
        //        CourseAdmin = course
        //    };

        //    ViewData["Title"] = "Enroll in Secondary Course";
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult SecondaryEnroll(int courseId)
        {
            // Get user email from session
            string userEmail = HttpContext.Session.GetString("UserEmail");

            // Check if email exists and if user is already enrolled
            if (!string.IsNullOrEmpty(userEmail))
            {
                bool isEnrolled = _context.secondaryEnrolls.Any(e =>
                    e.CourseId == courseId &&
                    e.Email == userEmail);

                // If already enrolled, redirect directly to Units
                if (isEnrolled)
                {
                    // Use direct return to ensure redirection happens
                    return RedirectToAction("Units", "Client", new { courseId });
                }
            }

            // Not enrolled, so show enrollment form
            var course = _context.courseAdmins.Find(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var model = new Models.SecondaryEnroll
            {
                CourseId = courseId,
                CourseAdmin = course,
                Email = userEmail ?? ""
            };

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



        //[HttpGet]
        //public IActionResult HigherSecondaryEnroll(int courseId)
        //{
        //    var course = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId); // Adjust if using CourseAdmin or another entity

        //    var model = new AstroSafar.Models.HigherSecondaryEnroll
        //    {
        //        CourseId = courseId,
        //        CourseAdmin = course // Initialize CourseAdmin to avoid null reference
        //    };

        //    ViewData["Title"] = "Enroll in Course";
        //    return View(model);
        //}
        [HttpGet]
        public IActionResult HigherSecondaryEnroll(int courseId)
        {
            // Get user email from session
            string userEmail = HttpContext.Session.GetString("UserEmail");

            // Check if email exists and if user is already enrolled
            if (!string.IsNullOrEmpty(userEmail))
            {
                bool isEnrolled = _context.higherSecondaryEnrolls.Any(e =>
                    e.CourseId == courseId &&
                    e.Email == userEmail);

                // If already enrolled, redirect directly to Units
                if (isEnrolled)
                {
                    // Use direct return to ensure redirection happens
                    return RedirectToAction("Units", "Client", new { courseId });
                }
            }

            // Not enrolled, so show enrollment form
            var course = _context.courseAdmins.Find(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var model = new AstroSafar.Models.HigherSecondaryEnroll
            {
                CourseId = courseId,
                CourseAdmin = course,
                Email = userEmail ?? ""
            };

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

        [HttpPost]
        public IActionResult CompleteUnit(int unitId)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail"); // Get user email from session

            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            var userProgress = _context.UnitProgresses
                .FirstOrDefault(p => p.Email == userEmail && p.UnitId == unitId);

            if (userProgress == null)
            {
                userProgress = new UnitProgress
                {
                    Email = userEmail,
                    UnitId = unitId
                };
                _context.UnitProgresses.Add(userProgress);
                _context.SaveChanges();
            }

            // Get courseId from unit
            var courseId = _context.unitAdmins
                .Where(u => u.Id == unitId)
                .Select(u => u.CourseId)
                .FirstOrDefault();

            // Count total units in the course
            var totalUnits = _context.unitAdmins
                .Where(u => u.CourseId == courseId)
                .Count();

            // Count completed units
            var completedUnits = _context.UnitProgresses
                .Where(p => p.Email == userEmail && _context.unitAdmins
                    .Any(u => u.Id == p.UnitId && u.CourseId == courseId))
                .Count();

            // Calculate progress (as a percentage)
            int overallProgress = (int)((completedUnits / (double)totalUnits) * 100);

            return Json(new { success = true, progress = overallProgress });
        }
        // Needed

        //[HttpPost]
        //public IActionResult UpdateProgress([FromBody] UpdateUnitProgressRequest request)
        //{
        //    var email = HttpContext.Session.GetString("UserEmail");

        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return Json(new { success = false, message = "User not logged in." });
        //    }

        //    var progress = _context.UnitProgresses
        //        .FirstOrDefault(up => up.Email == email && up.CourseId == request.CourseId && up.UnitId == request.UnitId);

        //    if (progress == null)
        //    {
        //        progress = new UnitProgress
        //        {
        //            Email = email,
        //            CourseId = request.CourseId,
        //            UnitId = request.UnitId,
        //            IsCompleted = false
        //        };
        //        _context.UnitProgresses.Add(progress);
        //    }

        //    // Mark unit as completed
        //    progress.IsCompleted = true;
        //    _context.SaveChanges();

        //    // ✅ Calculate overall course progress
        //    var totalUnits = _context.unitAdmins.Count(u => u.CourseId == request.CourseId);
        //    var completedUnits = _context.UnitProgresses.Count(up => up.Email == email && up.CourseId == request.CourseId && up.IsCompleted);

        //    int progressPercentage = (totalUnits > 0) ? (completedUnits * 100) / totalUnits : 0;

        //    // ✅ Unlock exam if progress is at least 80%
        //    if (progressPercentage >= 0)
        //    {
        //        HttpContext.Session.SetInt32($"UnlockExam_{request.CourseId}", 1);  // 1 means exam unlocked
        //    }

        //    return Json(new { success = true, progress = progressPercentage });
        //}


        //[HttpPost]
        //public IActionResult UpdateProgress([FromBody] UpdateUnitProgressRequest request)
        //{
        //    var email = HttpContext.Session.GetString("UserEmail");

        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return Json(new { success = false, message = "User not logged in." });
        //    }

        //     ✅ Fetch or create a UnitProgress entry
        //    var progress = _context.UnitProgresses
        //        .FirstOrDefault(up => up.Email == email && up.CourseId == request.CourseId && up.UnitId == request.UnitId);

        //    if (progress == null)
        //    {
        //        progress = new UnitProgress
        //        {
        //            Email = email,
        //            CourseId = request.CourseId,
        //            UnitId = request.UnitId,
        //            IsCompleted = false,
        //            ProgressPercentage = 0
        //        };
        //        _context.UnitProgresses.Add(progress);
        //    }

        //     ✅ Mark as completed
        //    progress.IsCompleted = true;
        //    _context.SaveChanges();

        //     ✅ Fetch the total number of units for this course dynamically
        //    var totalUnits = _context.unitAdmins
        //        .Where(u => u.CourseId == request.CourseId)
        //        .Count();

        //     ✅ Count the number of completed units by this user
        //    var completedUnits = _context.UnitProgresses
        //        .Where(up => up.Email == email && up.CourseId == request.CourseId && up.IsCompleted)
        //        .Count();

        //     ✅ Ensure progress does not exceed 100 %
        //    int progressPercentage = (totalUnits > 0) ? Math.Min((completedUnits * 100) / totalUnits, 100) : 0;

        //     ✅ Update progress percentage for all units
        //    var userProgressEntries = _context.UnitProgresses
        //        .Where(up => up.Email == email && up.CourseId == request.CourseId)
        //        .ToList();

        //    foreach (var entry in userProgressEntries)
        //    {
        //        entry.ProgressPercentage = progressPercentage;
        //    }

        //    _context.SaveChanges(); // Save progress

        //     ✅ Unlock exam if progress reaches 80 % +
        //    if (progressPercentage >= 80)
        //    {
        //        HttpContext.Session.SetInt32($"UnlockExam_{request.CourseId}", 1);
        //    }

        //    return Json(new { success = true, progress = progressPercentage });
        //}

        public IActionResult StudentDashboard()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            // Get total units enrolled by the student
            var totalUnits = _context.UnitProgresses.Count(up => up.Email == email);
            var completedUnits = _context.UnitProgresses.Count(up => up.Email == email && up.IsCompleted);

            // Calculate progress percentage
            var progressPercentage = (totalUnits > 0) ? (completedUnits * 100) / totalUnits : 0;

            // Fetch all courses student is enrolled in
            var enrolledCourses = _context.enrollments
                .Where(e => e.Email == email)
                .Select(e => e.CourseId)
                .ToList();

            // Build course progress list
            var courseProgressList = enrolledCourses.Select(courseId =>
            {
                var courseName = _context.courseAdmins
                    .Where(c => c.Id == courseId)
                    .Select(c => c.Name)
                    .FirstOrDefault();

                var courseTotalUnits = _context.unitAdmins.Count(u => u.CourseId == courseId);
                var courseCompletedUnits = _context.UnitProgresses.Count(up => up.Email == email && up.CourseId == courseId && up.IsCompleted);
                var progressPercentage = (courseTotalUnits > 0) ? (courseCompletedUnits * 100) / courseTotalUnits : 0;

                // ✅ Check if exam is unlocked
                bool isExamUnlocked = HttpContext.Session.GetInt32($"UnlockExam_{courseId}") == 1;

                return new CourseProgressViewModel
                {
                    CourseName = courseName,
                    ProgressPercentage = progressPercentage,
                   // IsExamUnlocked = isExamUnlocked
                };
            }).ToList();

            // Pass data to ViewModel
            var model = new StudentDashboardViewModel
            {
                ProgressPercentage = progressPercentage,
                CourseProgressList = courseProgressList
            };

            return View(model);
        }




    }
}