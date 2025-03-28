using AstroSafar.Migrations;
using AstroSafar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.RegularExpressions;


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

        [HttpGet]
        public IActionResult GetUserProgress(int courseId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            var totalUnits = _context.unitAdmins.Count(u => u.CourseId == courseId);
            var completedUnits = _context.UnitProgresses
                .Count(up => up.Email == email &&
                             up.CourseId == courseId &&
                             up.IsCompleted);

            int progressPercentage = (totalUnits > 0)
                ? (int)Math.Round((double)(completedUnits * 100) / totalUnits)
                : 0;

            var completedUnitIds = _context.UnitProgresses
                .Where(up => up.Email == email &&
                            up.CourseId == courseId &&
                            up.IsCompleted)
                .Select(up => up.UnitId)
                .ToList();

            return Json(new
            {
                success = true,
                courseProgress = progressPercentage,
                completedUnits = completedUnits,
                totalUnits = totalUnits,
                completedUnitIds = completedUnitIds
            });
        }
        [HttpPost]
        public IActionResult UpdateProgress([FromBody] UnitProgressRequest request)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            // Find or create progress record
            var progress = _context.UnitProgresses
                .FirstOrDefault(up => up.Email == email &&
                                     up.CourseId == request.CourseId &&
                                     up.UnitId == request.UnitId);

            if (progress == null)
            {
                progress = new UnitProgress
                {
                    Email = email,
                    CourseId = request.CourseId,
                    UnitId = request.UnitId,
                    IsCompleted = request.IsCompleted,
                    ProgressPercentage = request.IsCompleted ? 100 : 0
                };
                _context.UnitProgresses.Add(progress);
            }
            else
            {
                progress.IsCompleted = request.IsCompleted;
                progress.ProgressPercentage = request.IsCompleted ? 100 : 0;
            }

            _context.SaveChanges();

            // Calculate total and completed units for this course and user
            var totalUnits = _context.unitAdmins.Count(u => u.CourseId == request.CourseId);
            var completedUnits = _context.UnitProgresses
                .Count(up => up.Email == email &&
                             up.CourseId == request.CourseId &&
                             up.IsCompleted);

            // Calculate progress percentage
            int progressPercentage = (totalUnits > 0)
                ? (int)Math.Round((double)(completedUnits * 100) / totalUnits)
                : 0;

            // Store progress in session
            HttpContext.Session.SetInt32($"Progress_{request.CourseId}", progressPercentage);

            return Json(new
            {
                success = true,
                courseProgress = progressPercentage,
                completedUnits = completedUnits,
                totalUnits = totalUnits
            });
        }

        [HttpGet]
        public IActionResult GetCompletedUnits()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "User not logged in." });
            }

            // Get current course ID from URL or session
            var currentPath = HttpContext.Request.Path.ToString();
            var courseMatch = Regex.Match(currentPath, @"/Course/(\d+)");
            var courseId = courseMatch.Success ? int.Parse(courseMatch.Groups[1].Value) : 0;

            if (courseId == 0)
            {
                return Json(new { success = false, message = "Course ID not found." });
            }

            var completedUnits = _context.UnitProgresses
                .Where(up => up.Email == email &&
                           up.CourseId == courseId &&
                           up.IsCompleted)
                .Select(up => up.UnitId.ToString())
                .ToList();

            return Json(new { success = true, completedUnits });
        }
        public IActionResult ExamProgress(int courseId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var course = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }

            // Get all units for this course
            var courseUnits = _context.unitAdmins
                .Where(u => u.CourseId == courseId)
                .ToList();

            // Get completed units for this user and course
            var completedUnits = _context.UnitProgresses
                .Where(up => up.Email == email &&
                            up.CourseId == courseId &&
                            up.IsCompleted)
                .ToList();

            // Calculate progress
            int totalUnits = courseUnits.Count;
            double progressPerUnit = totalUnits > 0 ? 100.0 / totalUnits : 0;
            int overallProgress = (int)(completedUnits.Count * progressPerUnit);
            overallProgress = Math.Min(overallProgress, 100);

            // Check if all units are completed
            bool allUnitsCompleted = totalUnits > 0 && completedUnits.Count == totalUnits;

            ViewBag.CourseId = courseId;
            ViewBag.CourseName = course.Name;
            ViewBag.Progress = overallProgress;
            ViewBag.CompletedUnits = completedUnits.Count;
            ViewBag.TotalUnits = totalUnits;
            ViewBag.AllUnitsCompleted = allUnitsCompleted; // New flag for complete status

            return View();
        }
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

        public IActionResult ViewExam(int courseId)
        {
            // Fetch the exam questions related to the selected course only
            var questions = _context.ExamQuestions
                .Where(q => q.CourseId == courseId) // Filter by CourseId
                .ToList();

            ViewBag.CourseName = _context.courseAdmins.FirstOrDefault(c => c.Id == courseId)?.Name;

            return View(questions);
        }



    }
}
    
    
        
    