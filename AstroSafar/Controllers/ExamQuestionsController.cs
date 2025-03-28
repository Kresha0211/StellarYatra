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
        public async Task<IActionResult> Index()
        {
            var questions = _context.ExamQuestions.ToList();
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
                return RedirectToAction("Login", "Account"); // Redirect if session is missing
            }

            // Find enrolled course using email
            var enrolledCourseId = _context.enrollments
                .Where(e => e.Email == userEmail)
                .Select(e => e.CourseId)
                .FirstOrDefault();

            if (enrolledCourseId == 0)
            {
                return RedirectToAction("ExamProgress"); // Redirect if no enrolled course found
            }



            // Fetch exam questions for the selected course
            var questions = _context.ExamQuestions
                .Where(q => q.CourseId == courseId)
                .Select(q => new QuestionInputModel
                {
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Options = q.Options, // Comma-separated values
                    CorrectAnswer = ""  // Hide correct answer during exam
                }).ToList();

            // Log the number of questions fetched
            Console.WriteLine("Number of questions fetched: " + questions.Count);

            // Create ViewModel
            var examViewModel = new ExamQuestionsViewModel
            {
                CourseId = courseId,
                Questions = questions
            };

            Console.WriteLine("Exam ViewModel created successfully.");
            return View(examViewModel);
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
