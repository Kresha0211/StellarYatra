using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AstroSafar.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;
            return View();
        }
        private readonly SpaceLearningDBContext _context;

        public AdminController(SpaceLearningDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.AdminLogins.FirstOrDefault(a => a.Email == model.Email);

                if (admin != null && admin.Password == model.Password)  // For simplicity, no hashing
                {
                    return RedirectToAction("AdminDashboard");
                }

                ViewBag.Error = "Invalid Email or Password";
            }

            return View(model);
        }

        public IActionResult AdminDashboard()
        {

            var categories = _context.Categories.ToList(); // Fetch categories from the database

            if (categories == null || !categories.Any())
            {
                ViewBag.Message = "No categories available.";
            }

            return View(categories); // Pass categories to the view

        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        // Add Category
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddCategory(Category model)
        {

            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                return RedirectToAction("CategoryList");
            }

            // Debugging - Check if ModelState is invalid
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(model);
        }
        public IActionResult CategoryList()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // Edit Category
       
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Categories.Find(model.Id);
                if (category == null)
                {
                    return NotFound();
                }

                category.Name = model.Name;
                
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("CategoryList");

            }

            return View(model);
        }

        // Delete Category
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("CategoryList");
        }


        // Add Course
        [HttpGet]
        public IActionResult AddCourse()
        {
            var categories = _context.Categories
                              .Select(c => new { c.Id, c.Name })
                              .ToList();

            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddCourse(CourseAdmin course)
        {

            {
                _context.courseAdmins.Add(course);
                _context.SaveChanges();
                return RedirectToAction("CourseList");
            }

            // Debugging - Check if ModelState is invalid
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(course);
        }
        public IActionResult CourseList()
        {
            // Fetch courses along with their categories
            var courses = _context.courseAdmins
                .Include(c => c.Category) // Ensure Category navigation property is included
                .ToList();

            return View(courses);
        }


        // Edit Course
        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var course = _context.courseAdmins.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(course);
        }

        [HttpPost]
        public IActionResult EditCourse(CourseAdmin model)
        {
            var course = _context.courseAdmins.Find(model.Id);

            // Update the necessary properties
            course.Name = model.Name;
            course.Description = model.Description;
            course.Duration = model.Duration;
            course.CategoryId = model.CategoryId;

            _context.SaveChanges();  // Save changes to the database

            return RedirectToAction("CourseList");   // Return the view with model if validation fails
        }

        // Delete Course
        [HttpPost]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.courseAdmins.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.courseAdmins.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("CourseList");
        }

        // Unit
        [HttpGet]
        public IActionResult AddUnit()
        {
            // Fetch courses for the dropdown
            var courses = _context.courseAdmins.Select(c => new { c.Id, c.Name }).ToList();

            // Pass the list to the view
            ViewBag.Courses = courses;

            // Optional: Fetch existing units if needed
            var units = _context.unitAdmins
                .Select(u => new { u.Id, u.Name, CourseName = u.CourseAdmin.Name })
                .ToList();

            ViewBag.Units = units;

            return View();
        }


        [HttpPost]
        public IActionResult AddUnit(UnitAdmin unit)
        {

            {
                _context.unitAdmins.Add(unit);
                _context.SaveChanges();
                return RedirectToAction("UnitList");
            }

            // Debugging - Check if ModelState is invalid
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(unit);
        }
        public IActionResult UnitList()
        {
            var units = _context.unitAdmins
           .Include(u => u.CourseAdmin) // Ensure Course is loaded
           .Select(u => new UnitAdmin
           {
               Id = u.Id,
               Name = u.Name,
               VideoUrl = u.VideoUrl,
               CourseAdmin = u.CourseAdmin  // Assign actual Course object
           })
           .ToList();
            ViewBag.Message = "List of Units for the selected Course";  // For example, a custom message
            ViewBag.CourseName = units.FirstOrDefault()?.CourseAdmin.Name ?? "No Course Selected"; // Assign course name dynamically

            return View(units);
        }

        // Edit Unit

        [HttpGet]
        public IActionResult EditUnit(int id)
        {
            // Fetch the unit with its associated course for editing
            var unit = _context.unitAdmins
                .Include(u => u.CourseAdmin) // Load the course related to the unit
                .FirstOrDefault(u => u.Id == id);

            if (unit == null)
            {
                return NotFound();
            }

            // Fetch courses for the dropdown
            var courses = _context.courseAdmins.Select(c => new { c.Id, c.Name }).ToList();
            ViewBag.Courses = courses;

            return View(unit);  // Pass the unit to the view for editing
        }
        [HttpPost]
        public IActionResult EditUnit(UnitAdmin unit)
        {
            // Find the existing unit and update its properties
            var existingUnit = _context.unitAdmins.Find(unit.Id);
            existingUnit.Name = unit.Name;
            existingUnit.VideoUrl = unit.VideoUrl;
            existingUnit.CourseAdmin = unit.CourseAdmin; // Update the course

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("UnitList"); // Return the view with model if validation fails
        }


        // Delete Unit
        [HttpPost]
        public IActionResult DeleteUnit(int id)
        {
            var unit = _context.unitAdmins.Find(id);
            if (unit == null)
            {
                return NotFound();
            }

            _context.unitAdmins.Remove(unit);
            _context.SaveChanges();

            return RedirectToAction("UnitList");
        }

    }
}
