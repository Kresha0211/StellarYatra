

//Needed

//    using Microsoft.AspNetCore.Mvc;
//    using AstroSafar.Models;
//    using System.Linq;
//    using System.Threading.Tasks;
//    using Microsoft.EntityFrameworkCore;
//    using AstroSafar.Migrations;

//    namespace AstroSafar.Controllers
//    {
//        public class ExamQuestions : Controller
//        {
//            private readonly SpaceLearningDBContext _context;

//            public ExamQuestions(SpaceLearningDBContext context)
//            {
//                _context = context;
//            }

//            // Show All Questions
//            public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
//            {
//                var totalQuestions = await _context.ExamQuestions.CountAsync();
//                var questions = await _context.ExamQuestions
//                    .OrderBy(q => q.Id)
//                    .Skip((page - 1) * pageSize)
//                    .Take(pageSize)
//                    .ToListAsync();

//                ViewBag.CurrentPage = page;
//                ViewBag.TotalPages = (int)Math.Ceiling((double)totalQuestions / pageSize);

//                return View(questions);
//            }

//            // GET: Add Exam
//            public IActionResult AddExam()
//            {
//                // Fetching courses from the database to populate the dropdown
//                ViewBag.Courses = _context.courseAdmins.ToList();

//                // Initializing an empty view model with one question for dynamic addition
//                var model = new ExamQuestionsViewModel
//                {
//                    Questions = new List<QuestionInputModel> { new QuestionInputModel() }
//                };

//                return View(model);
//            }
//            [HttpPost]
//            public IActionResult AddExam(ExamQuestionsViewModel model)
//            {
//                try
//                {
//                    foreach (var question in model.Questions)
//                    {
//                        var newQuestion = new ExamQuestion
//                        {
//                            CourseId = model.CourseId,
//                            QuestionText = question.QuestionText,
//                            QuestionType = question.QuestionType,
//                            Options = question.QuestionType == "MCQ" ? question.Options : "True,False",
//                            CorrectAnswer = question.CorrectAnswer
//                        };

//                        _context.ExamQuestions.Add(newQuestion);
//                    }

//                    _context.SaveChanges();
//                    TempData["Success"] = "Questions added successfully!";
//                    return RedirectToAction("Index");
//                }
//                catch (Exception ex)
//                {
//                    // Log the error for debugging purposes
//                    Console.WriteLine("Error adding questions: " + ex.Message);
//                    TempData["Error"] = "An error occurred while adding questions.";
//                    return View(model);
//                }
//            }

//            [HttpGet]
//            public IActionResult StartExam(int courseId)
//            {
//                // Get user's email from session
//                string userEmail = HttpContext.Session.GetString("UserEmail");
//                Console.WriteLine("User Email from Session: " + userEmail);
//                Console.WriteLine("Course ID: " + courseId);

//                if (string.IsNullOrEmpty(userEmail))
//                {
//                    Console.WriteLine("User email not found in session. Redirecting to Login.");
//                    return RedirectToAction("Login", "Account"); // Redirect if session is missing
//                }

//                // Find enrolled course using email
//                var enrollment = _context.enrollments
//                    .Where(e => e.Email == userEmail && e.CourseId == courseId)
//                    .FirstOrDefault();

//                if (enrollment == null)
//                {
//                    return RedirectToAction("ExamProgress"); // Redirect if no enrolled course found
//                }

//                // Check if user has already completed the exam for this course
//                var existingResult = _context.ExamResults
//                    .Where(r => r.EnrollmentId == enrollment.Id && r.IsCompleted)
//                    .FirstOrDefault();

//                TempData["enId"] = enrollment.Id;

//                if (existingResult != null)
//                {
//                    // User has already completed the exam - redirect to certificate prompt
//                    TempData["Message"] = "You have already completed this exam. You cannot retake it.";
//                    return RedirectToAction("CertificatePrompt", "Certificate");
//                }

//                // Fetch exam questions for the selected course
//                var questions = _context.ExamQuestions
//                    .Where(q => q.CourseId == courseId)
//                    .Select(q => new QuestionInputModel
//                    {
//                        QuestionId = q.Id,  // Include the question ID
//                        QuestionText = q.QuestionText,
//                        QuestionType = q.QuestionType,
//                        Options = q.Options, // Comma-separated values
//                        CorrectAnswer = ""  // Hide correct answer during exam
//                    }).ToList();

//                // Log the number of questions fetched
//                Console.WriteLine("Number of questions fetched: " + questions.Count);

//                // Create ViewModel
//                var examViewModel = new ExamQuestionsViewModel
//                {
//                    CourseId = courseId,
//                    Questions = questions
//                };

//                Console.WriteLine("Exam ViewModel created successfully.");
//                return View(examViewModel);
//            }


//            [HttpPost]
//            public async Task<IActionResult> StartExam(ExamSubmissionViewModel model)
//            {
//                int score = await CalculateScoreAsync(model.UserAnswers); // Calculate score asynchronously

//                // Save result to DB
//                var examResult = new ExamResult
//                {
//                    EnrollmentId = model.EnrollmentId,
//                    Score = score,
//                    IsCompleted = true,
//                    CompletedAt = DateTime.Now
//                };

//                _context.ExamResults.Add(examResult);
//                await _context.SaveChangesAsync(); // Async save

//                // Store score in TempData
//                TempData["Score"] = score;

//                return RedirectToAction("CertificatePrompt", "Certificate");
//            }




//            [HttpPost]
//            public async Task<IActionResult> SubmitExam(ExamSubmissionViewModel model)
//            {
//                string userEmail = HttpContext.Session.GetString("UserEmail");

//                if (string.IsNullOrEmpty(userEmail))
//                {
//                    return RedirectToAction("Login", "Account");
//                }

//                var enrollment = _context.enrollments
//                    .FirstOrDefault(e => e.Email == userEmail && e.CourseId == model.CourseId);

//                if (enrollment == null)
//                {
//                    return RedirectToAction("ExamProgress");
//                }

//                // ✅ This must be awaited and call the correct method
//                int score = await CalculateScoreAsync(model.UserAnswers); // ✅

//                var examResult = new ExamResult
//                {
//                    EnrollmentId = enrollment.Id,
//                    IsCompleted = true,
//                    CompletedAt = DateTime.Now,
//                    Score = score,
//                    TimeTaken = TimeSpan.FromMinutes(30) - TimeSpan.FromSeconds(model.TimeRemaining)
//                };

//                _context.ExamResults.Add(examResult);
//                await _context.SaveChangesAsync();

//                TempData["Score"] = score; // ✅ Store score for next page

//                return RedirectToAction("CertificatePrompt", "Certificate");
//            }

//            private async Task<int> CalculateScoreAsync(List<UserAnswer> answers)
//            {
//                int totalScore = 0;

//                foreach (var userAnswer in answers)
//                {
//                    // Debug log
//                    Console.WriteLine($"Looking for question with ID: {userAnswer.QuestionId}");

//                    var question = await _context.ExamQuestions.FindAsync(userAnswer.QuestionId);

//                    if (question != null)
//                    {
//                        Console.WriteLine($"Found question: {question.QuestionText}");
//                        Console.WriteLine($"Correct answer: {question.CorrectAnswer}, User selected: {userAnswer.SelectedOption}");

//                        if (question.CorrectAnswer == userAnswer.SelectedOption)
//                        {
//                            totalScore += 2;
//                            Console.WriteLine("Answer is correct! +2 points");
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine($"Question with ID {userAnswer.QuestionId} not found!");
//                    }
//                }

//                Console.WriteLine($"Final score: {totalScore}");
//                return totalScore;
//            }




//            // GET: Edit Question
//            public async Task<IActionResult> EditExam(int id)
//            {
//                var question = await _context.ExamQuestions.FindAsync(id);
//                if (question == null) return NotFound();

//                ViewBag.Courses = await _context.courseAdmins.ToListAsync();
//                return View(question);
//            }

//            // POST: Edit Question
//            [HttpPost]
//            public async Task<IActionResult> EditExam(ExamQuestion question)
//            {
//                _context.ExamQuestions.Update(question);
//                await _context.SaveChangesAsync();
//                return RedirectToAction("Index");
//            }
//            // DELETE: Delete Question
//            public async Task<IActionResult> DeleteExam(int id)
//            {
//                var question = await _context.ExamQuestions.FindAsync(id);
//                if (question != null)
//                {
//                    _context.ExamQuestions.Remove(question);
//                    await _context.SaveChangesAsync();
//                }
//                return RedirectToAction("Index");
//            }

//            public IActionResult ExamList()
//            {
//                var questions = _context.ExamQuestions.Include(q => q.Course).ToList();
//                return View(questions);
//            }
//    }
//}



using Microsoft.AspNetCore.Mvc;
using AstroSafar.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AstroSafar.Migrations;

namespace AstroSafar.Controllers
{
    public class ExamQuestions : Controller
    {
        private readonly SpaceLearningDBContext _context;

        public ExamQuestions(SpaceLearningDBContext context)
        {
            _context = context;
        }

        // Show All Questions
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var totalQuestions = await _context.ExamQuestions.CountAsync();
            var questions = await _context.ExamQuestions
                .OrderBy(q => q.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalQuestions / pageSize);

            return View(questions);
        }

        // GET: Add Exam
        public IActionResult AddExam()
        {
            // Fetching courses from the database to populate the dropdown
            ViewBag.Courses = _context.courseAdmins.ToList();

            // Initializing an empty view model with one question for dynamic addition
            var model = new ExamQuestionsViewModel
            {
                Questions = new List<QuestionInputModel> { new QuestionInputModel() }
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult AddExam(ExamQuestionsViewModel model)
        {
            try
            {
                foreach (var question in model.Questions)
                {
                    var newQuestion = new ExamQuestion
                    {
                        CourseId = model.CourseId,
                        QuestionText = question.QuestionText,
                        QuestionType = question.QuestionType,
                        Options = question.QuestionType == "MCQ" ? question.Options : "True,False",
                        CorrectAnswer = question.CorrectAnswer
                    };

                    _context.ExamQuestions.Add(newQuestion);
                }

                _context.SaveChanges();
                TempData["Success"] = "Questions added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine("Error adding questions: " + ex.Message);
                TempData["Error"] = "An error occurred while adding questions.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult StartExam(int courseId)
        {
            // Get user's email from session
            string userEmail = HttpContext.Session.GetString("UserEmail");
            Console.WriteLine("User Email from Session: " + userEmail);
            Console.WriteLine("Course ID: " + courseId);

            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("User email not found in session. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }

            // 1️⃣ Check Main Enrollment
            var enrollment = _context.enrollments
                .FirstOrDefault(e => e.Email == userEmail && e.CourseId == courseId);

            // 2️⃣ Check Secondary Enrollment
            var secondaryEnroll = _context.secondaryEnrolls
                .FirstOrDefault(se => se.Email == userEmail && se.CourseId == courseId);

            // 3️⃣ Check Higher Secondary Enrollment
            var higherEnroll = _context.higherSecondaryEnrolls
                .FirstOrDefault(he => he.Email == userEmail && he.CourseId == courseId);

            // Check which one is valid
            if (enrollment == null && secondaryEnroll == null && higherEnroll == null)
            {
                return RedirectToAction("ExamProgress");
            }

            int? enrollmentId = null;

            // Store the appropriate enrollment ID
            if (enrollment != null)
            {
                enrollmentId = enrollment.Id;
                TempData["EnrollmentType"] = "Main";
            }
            else if (secondaryEnroll != null)
            {
                enrollmentId = secondaryEnroll.Id;
                TempData["EnrollmentType"] = "Secondary";
            }
            else if (higherEnroll != null)
            {
                enrollmentId = higherEnroll.Id;
                TempData["EnrollmentType"] = "HigherSecondary";
            }

            // Only set TempData if we have a valid enrollment ID
            if (enrollmentId.HasValue)
            {
                TempData["enId"] = enrollmentId.Value;
                TempData["EnrollmentId"] = enrollmentId.Value;
            }

            // 4️⃣ Check if user already completed the exam
            var existingResult = _context.ExamResults
                .FirstOrDefault(r =>
                    (enrollment != null && r.EnrollmentId == enrollment.Id && r.IsCompleted) ||
                    (secondaryEnroll != null && r.SecondaryEnrollId == secondaryEnroll.Id && r.IsCompleted) ||
                    (higherEnroll != null && r.HigherSecondaryEnrollId == higherEnroll.Id && r.IsCompleted)
                );

            if (existingResult != null)
            {
                TempData["Message"] = "You have already completed this exam. You cannot retake it.";
                return RedirectToAction("CertificatePrompt", "Certificate");
            }

            // 6️⃣ Fetch exam questions
            var questions = _context.ExamQuestions
                .Where(q => q.CourseId == courseId)
                .Select(q => new QuestionInputModel
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options,
                    CorrectAnswer = ""
                }).ToList();

            Console.WriteLine("Number of questions fetched: " + questions.Count);

            var examViewModel = new ExamQuestionsViewModel
            {
                CourseId = courseId,
                Questions = questions
            };

            return View(examViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> StartExam(ExamSubmissionViewModel model)
        {
            int score = await CalculateScoreAsync(model.UserAnswers);

            string enrollmentType = TempData["EnrollmentType"]?.ToString();
            int enrollmentId = Convert.ToInt32(TempData["EnrollmentId"]);

            var examResult = new ExamResult
            {
                Score = score,
                IsCompleted = true,
                CompletedAt = DateTime.Now
            };

            // Set correct enrollment FK
            switch (enrollmentType)
            {
                case "Main":
                    examResult.EnrollmentId = enrollmentId;
                    break;
                case "Secondary":
                    examResult.SecondaryEnrollId = enrollmentId;
                    break;
                case "HigherSecondary":
                    examResult.HigherSecondaryEnrollId = enrollmentId;
                    break;
            }

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();

            TempData["Score"] = score;
            return RedirectToAction("CertificatePrompt", "Certificate");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(ExamSubmissionViewModel model)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollment = _context.enrollments
                .FirstOrDefault(e => e.Email == userEmail && e.CourseId == model.CourseId);
            var secondaryEnroll = _context.secondaryEnrolls
                .FirstOrDefault(e => e.Email == userEmail && e.CourseId == model.CourseId);
            var higherSecondaryEnroll = _context.higherSecondaryEnrolls
                .FirstOrDefault(e => e.Email == userEmail && e.CourseId == model.CourseId);

            // No valid enrollment in any category
            if (enrollment == null && secondaryEnroll == null && higherSecondaryEnroll == null)
            {
                return RedirectToAction("ExamProgress");
            }

            int score = await CalculateScoreAsync(model.UserAnswers);

            var examResult = new ExamResult
            {
                IsCompleted = true,
                CompletedAt = DateTime.Now,
                Score = score,
                TimeTaken = TimeSpan.FromMinutes(30) - TimeSpan.FromSeconds(model.TimeRemaining)
            };

            // Set correct foreign key
            if (enrollment != null)
                examResult.EnrollmentId = enrollment.Id;
            else if (secondaryEnroll != null)
                examResult.SecondaryEnrollId = secondaryEnroll.Id;
            else if (higherSecondaryEnroll != null)
                examResult.HigherSecondaryEnrollId = higherSecondaryEnroll.Id;

            _context.ExamResults.Add(examResult);
            await _context.SaveChangesAsync();

            TempData["Score"] = score;
            TempData["CourseId"] = model.CourseId;

            // Redirect to the correct result view based on enrollment type
            return RedirectToAction("CertificatePrompt", "Certificate");
        }

        private async Task<int> CalculateScoreAsync(List<UserAnswer> answers)
        {
            int totalScore = 0;

            foreach (var userAnswer in answers)
            {
                // Debug log
                Console.WriteLine($"Looking for question with ID: {userAnswer.QuestionId}");

                var question = await _context.ExamQuestions.FindAsync(userAnswer.QuestionId);

                if (question != null)
                {
                    Console.WriteLine($"Found question: {question.QuestionText}");
                    Console.WriteLine($"Correct answer: {question.CorrectAnswer}, User selected: {userAnswer.SelectedOption}");

                    if (question.CorrectAnswer == userAnswer.SelectedOption)
                    {
                        totalScore += 2;
                        Console.WriteLine("Answer is correct! +2 points");
                    }
                }
                else
                {
                    Console.WriteLine($"Question with ID {userAnswer.QuestionId} not found!");
                }
            }

            Console.WriteLine($"Final score: {totalScore}");
            return totalScore;
        }

        // GET: Edit Question
        public async Task<IActionResult> EditExam(int id)
        {
            var question = await _context.ExamQuestions.FindAsync(id);
            if (question == null) return NotFound();

            ViewBag.Courses = await _context.courseAdmins.ToListAsync();
            return View(question);
        }

        // POST: Edit Question
        [HttpPost]
        public async Task<IActionResult> EditExam(ExamQuestion question)
        {
            _context.ExamQuestions.Update(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // DELETE: Delete Question 
        public async Task<IActionResult> DeleteExam(int id)
        {
            var question = await _context.ExamQuestions.FindAsync(id);
            if (question != null)
            {
                _context.ExamQuestions.Remove(question);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ExamList()
        {
            var questions = _context.ExamQuestions.Include(q => q.Course).ToList();
            return View(questions);
        }
    }
}