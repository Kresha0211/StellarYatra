using AstroSafar.Models;
using Microsoft.AspNetCore.Authentication;
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
        private string? category;

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = _context.AdminLogins.FirstOrDefault(a => a.Email == model.Email);

            if (admin != null && admin.Password == model.Password)  // Consider hashing passwords
            {
                HttpContext.Session.SetString("AdminLoggedIn", "true"); // Set session only after successful login
                return RedirectToAction("AdminDashboard");
            }

            ViewBag.Error = "Invalid Email or Password";
            return View(model);
        }


        public IActionResult AdminDashboard()
        {

            var categories = _context.Categories.ToList(); // Fetch categories from the database

            if (categories == null || !categories.Any())
            {
                ViewBag.Message = "No categories available.";
            }

            ViewBag.TotalUsers = _context.Registrations.Count();
            ViewBag.TotalCourses = _context.courseAdmins.Count();
            ViewBag.TotalCertificates = _context.Certificates.Count();


            var totalBookBuyers = _context.BookOrders
       .Select(o => o.UserId)
       .Distinct()
       .Count();
            ViewBag.TotalBookBuyers = totalBookBuyers;
            ViewBag.RecentEnrollments = _context.enrollments
                .OrderByDescending(e => e.Id)
                .Take(5)
                .Select(e => new
                {
                    UserName = e.FullName,
                    CourseName = e.CourseAdmin.Name,
                    //EnrollmentDate = e.EnrollmentDate
                }).ToList();



            return View(categories);

        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Admin");

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
        // Select Category
        public IActionResult CategoryDetails(int categoryId)
        {
            // Fetch courses only for the selected category
            var courses = _context.courseAdmins
                                  .Where(c => c.CategoryId == categoryId)
                                  .ToList();

            var units = _context.unitAdmins
                                .Where(u => u.CourseAdmin.CategoryId == categoryId)
                                .ToList();

            var viewModel = new CategoryDetailsViewModel
            {
                Courses = courses,
                Units = units,
                CategoryId = categoryId
            };

            return View(viewModel);
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
        //public IActionResult CourseList()
        //{
        //    // Fetch courses along with their categories
        //    var courses = _context.courseAdmins
        //        .Include(c => c.Category) // Ensure Category navigation property is included
        //        .ToList();

        //    return View(courses);
        //}

        //public IActionResult CourseList(string categoryFilter = "All")
        //{
        //    // Fetch courses along with their categories
        //    var courses = _context.courseAdmins
        //        .Include(c => c.Category)
        //        .AsQueryable();

        //    // Apply filter if not showing all
        //    if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All")
        //    {
        //        courses = courses.Where(c => c.Category.Name == categoryFilter);
        //    }

        //    // Pass selected category to the view
        //    ViewBag.SelectedCategory = categoryFilter;

        //    return View(courses.ToList());
        //}

        public IActionResult CourseList(int categoryFilter = 0, int page = 1, int pageSize = 5)
        {
            var courses = _context.courseAdmins
                .Include(c => c.Category)
                .AsQueryable();

            if (categoryFilter != 0)
            {
                courses = courses.Where(c => c.CategoryId == categoryFilter);
            }

            var totalCourses = courses.Count();
            var totalPages = (int)Math.Ceiling((double)totalCourses / pageSize);

            var paginatedCourses = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategory = categoryFilter;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedCourses);
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
            course.ImageURL = model.ImageURL;

            _context.SaveChanges();

            return RedirectToAction("CourseList");
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

        [HttpGet]
        public IActionResult AddUnit()
        {
            // Fetch courses for the dropdown
            var courses = _context.courseAdmins.Select(c => new { c.Id, c.Name }).ToList();


            ViewBag.Courses = courses;

            var units = _context.unitAdmins
                .Select(u => new { u.Id, u.Name, CourseName = u.CourseAdmin.Name })
                .ToList();

            ViewBag.Units = units;

            return View();
        }
        [HttpPost]
        public IActionResult AddUnit(UnitAdmin unit, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Save image path in the database
                unit.ImageURL = "/Images/" + fileName;
            }

            _context.unitAdmins.Add(unit);
            _context.SaveChanges();

            return RedirectToAction("UnitList");
        }

        //public IActionResult UnitList()
        //{
        //    var units = _context.unitAdmins
        //   .Include(u => u.CourseAdmin)
        //   .Select(u => new UnitAdmin
        //   {
        //       Id = u.Id,
        //       Name = u.Name,
        //       Content = u.Content,  // Ensure Content is fetched
        //       ImageURL = u.ImageURL,
        //       VideoUrl = u.VideoUrl,
        //       CourseAdmin = u.CourseAdmin
        //   })
        //   .ToList();
        //    ViewBag.Message = "List of Units for the selected Course";
        //    ViewBag.CourseName = units.FirstOrDefault()?.CourseAdmin.Name ?? "No Course Selected";

        //    return View(units);
        //}

        public IActionResult UnitList(int? courseFilter)
        {
            // Fetch all courses for the dropdown
            ViewBag.Courses = _context.courseAdmins
                                      .OrderBy(c => c.Name) // Optional: Alphabetical order
                                      .ToList();

            // Fetch and filter units
            var query = _context.unitAdmins.Include(u => u.CourseAdmin).AsQueryable();

            if (courseFilter.HasValue && courseFilter.Value != 0)
            {
                query = query.Where(u => u.CourseAdmin.Id == courseFilter.Value);
            }

            var units = query.Select(u => new UnitAdmin
            {
                Id = u.Id,
                Name = u.Name,
                Content = u.Content,
                ImageURL = u.ImageURL,
                VideoUrl = u.VideoUrl,
                CourseAdmin = u.CourseAdmin
            }).ToList();

            ViewBag.Message = "List of Units for the selected Course";
            ViewBag.CourseName = units.FirstOrDefault()?.CourseAdmin?.Name ?? "No Course Selected";
            ViewBag.SelectedCourse = courseFilter;

            return View(units);
        }




        // Edit Unit
        [HttpGet]
        public IActionResult EditUnit(int id)
        {
            // Fetch the unit with its associated course for editing
            var unit = _context.unitAdmins
                .Include(u => u.CourseAdmin) // Load the related course
                .FirstOrDefault(u => u.Id == id);

            if (unit == null)
            {
                return NotFound();
            }

            // Fetch courses for the dropdown
            var courses = _context.courseAdmins
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            ViewBag.Courses = courses; // Assign to ViewBag

            return View(unit);
        }


        [HttpPost]
        public IActionResult EditUnit(UnitAdmin model, IFormFile ImageFile)
        {
            var unit = _context.unitAdmins.Find(model.Id);
            if (unit == null)
            {
                return NotFound();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                unit.ImageURL = "/Images/" + uniqueFileName;
            }

            unit.Name = model.Name;
            unit.Content = model.Content;
            unit.VideoUrl = model.VideoUrl;
            unit.CourseId = model.CourseId;

            _context.Update(unit);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Unit updated successfully!";
            return RedirectToAction("UnitList");
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

        public IActionResult EnrolledUsers(string? categoryFilter = "All")
        {
            var primaryEnrollments = _context.enrollments
                .Include(e => e.CourseAdmin)
                    .ThenInclude(c => c.Category)
                .ToList();

            var secondaryEnrollments = _context.secondaryEnrolls
                .Include(e => e.CourseAdmin)
                    .ThenInclude(c => c.Category)
                .ToList();

            var higherSecondaryEnrollments = _context.higherSecondaryEnrolls
                .Include(e => e.CourseAdmin)
                    .ThenInclude(c => c.Category)
                .ToList();

            if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All")
            {
                switch (categoryFilter)
                {
                    case "Primary":
                        primaryEnrollments = primaryEnrollments
                            .Where(e => e.CourseAdmin.Category.Name == "Primary")
                            .ToList();
                        secondaryEnrollments = new List<SecondaryEnroll>();
                        higherSecondaryEnrollments = new List<HigherSecondaryEnroll>();
                        break;

                    case "Secondary":
                        secondaryEnrollments = secondaryEnrollments
                            .Where(e => e.CourseAdmin.Category.Name == "Secondary")
                            .ToList();
                        primaryEnrollments = new List<Enrollment>();
                        higherSecondaryEnrollments = new List<HigherSecondaryEnroll>();
                        break;

                    case "Higher Secondary":
                        higherSecondaryEnrollments = higherSecondaryEnrollments
                            .Where(e => e.CourseAdmin.Category.Name == "Higher Secondary")
                            .ToList();
                        primaryEnrollments = new List<Enrollment>();
                        secondaryEnrollments = new List<SecondaryEnroll>();
                        break;
                }
            }

            var model = new EnrolledUsersViewModel
            {
                enrollments = primaryEnrollments,
                SecondaryEnrolls = secondaryEnrollments,
                HigherSecondaryEnrolls = higherSecondaryEnrollments
            };

            ViewBag.SelectedCategory = categoryFilter;
            return View(model);
        }


        public IActionResult EnrollmentDetails(int id, string category)
        {
            object model = null;

            if (category == "Primary")
                model = _context.enrollments.FirstOrDefault(e => e.Id == id);
            else if (category == "Secondary")
                model = _context.secondaryEnrolls.FirstOrDefault(e => e.Id == id);
            else if (category == "HigherSecondary")
                model = _context.higherSecondaryEnrolls.FirstOrDefault(e => e.Id == id);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteEnrolledUser(int id)
        {
            var user = _context.enrollments.Find(id);
            if (user != null)
            {
                _context.enrollments.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("EnrolledUsers");
        }

        [HttpPost]
        public IActionResult DeleteSecondaryEnrolledUser(int id)
        {
            var user = _context.secondaryEnrolls.Find(id);
            if (user != null)
            {
                _context.secondaryEnrolls.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("EnrolledUsers");
        }

        [HttpPost]
        public IActionResult DeleteHigherSecondaryEnrolledUser(int id)
        {
            var user = _context.higherSecondaryEnrolls.Find(id);
            if (user != null)
            {
                _context.higherSecondaryEnrolls.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("EnrolledUsers");
        }


        public IActionResult UserProgress(int page = 1, int pageSize = 5)
        {
            var unitProgressList = (from unitProgress in _context.UnitProgresses
                                    join course in _context.courseAdmins
                                        on unitProgress.CourseId equals course.Id
                                    select new
                                    {
                                        unitProgress.Email,
                                        unitProgress.CourseId,
                                        unitProgress.IsCompleted,
                                        unitProgress.ProgressPercentage,
                                        CourseName = course.Name
                                    }).ToList();

            var groupedProgress = unitProgressList
                .GroupBy(x => new { x.Email, x.CourseId })
                .Select(g =>
                {
                    var email = g.Key.Email;
                    var courseId = g.Key.CourseId;

                    var score = (from er in _context.ExamResults
                                 join en in _context.enrollments
                                     on er.EnrollmentId equals en.Id
                                 where en.Email == email && en.CourseId == courseId
                                 select er.Score).FirstOrDefault();

                    return new
                    {
                        UserName = email,
                        Email = email,
                        CourseName = g.First().CourseName,
                        TotalUnits = g.Count(),
                        CompletedUnits = g.Count(x => x.IsCompleted),
                        ProgressPercentage = g.Average(x => x.ProgressPercentage),
                        Score = score
                    };
                }).ToList();

            var totalItems = groupedProgress.Count;
            var pagedProgress = groupedProgress
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            ViewBag.ProgressData = pagedProgress;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View();
        }


        public IActionResult Transactions(int page = 1, int pageSize = 5)
        {
            var allTransactions = (from t in _context.Transactions
                                   join e in _context.enrollments on t.EnrollmentId equals e.Id into enrollmentGroup
                                   from e in enrollmentGroup.DefaultIfEmpty()
                                   join u in _context.Registrations on e.Email equals u.Email into userGroup
                                   from u in userGroup.DefaultIfEmpty()
                                   join c in _context.courseAdmins on e.CourseId equals c.Id into courseGroup
                                   from c in courseGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       TransactionId = t.Id,
                                       UserName = u != null ? u.Firstname : "Unknown",
                                       Email = u != null ? u.Email : "No Email",
                                       CourseName = c != null ? c.Name : "No Course",
                                       Amount = t.Amount,
                                       Currency = t.Currency ?? "N/A",
                                       PaymentDate = t.PaymentDate,
                                       RazorpayPaymentId = t.RazorpayPaymentId ?? "N/A"
                                   }).ToList();

            int total = allTransactions.Count();
            int totalPages = (int)Math.Ceiling((double)total / pageSize);

            var paginatedTransactions = allTransactions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TransactionData = paginatedTransactions;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View();
        }


        public IActionResult IssuedCertificates(int page = 1, int pageSize = 5)
        {
            var allCertificates = (from cert in _context.Certificates
                                   join enroll in _context.enrollments on cert.EnrollmentId equals enroll.Id into enrollmentGroup
                                   from enroll in enrollmentGroup.DefaultIfEmpty()
                                   join user in _context.Registrations on enroll.Email equals user.Email into userGroup
                                   from user in userGroup.DefaultIfEmpty()
                                   join course in _context.courseAdmins on enroll.CourseId equals course.Id into courseGroup
                                   from course in courseGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       CertificateId = cert.Id,
                                       UserName = user != null ? user.Firstname : "Unknown",
                                       Email = user != null ? user.Email : "No Email",
                                       CourseName = course != null ? course.Name : "No Course",
                                       IssuedOn = cert.IssuedOn,
                                       CertificateNumber = cert.CertificateNumber ?? "N/A",
                                   }).ToList();

            int totalCertificates = allCertificates.Count();
            int totalPages = (int)Math.Ceiling((double)totalCertificates / pageSize);

            var pagedCertificates = allCertificates
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.IssuedCertificates = pagedCertificates;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View();
        }


        [HttpGet]
        public JsonResult GetEnrollmentDistribution()
        {
            var primary = _context.enrollments.Count();
            var secondary = _context.secondaryEnrolls.Count();
            var higherSecondary = _context.higherSecondaryEnrolls.Count();

            var result = new
            {
                labels = new[] { "Primary", "Secondary", "Higher Secondary" },
                counts = new[] { primary, secondary, higherSecondary }
            };

            return Json(result);
        }
        public IActionResult BookOrders(int page = 1, int pageSize = 2)
        {
            // Get total count for pagination
            int total = _context.BookOrders.Count();
            int totalPages = (int)Math.Ceiling((double)total / pageSize);

            // Get paginated data
            var orders = _context.BookOrders
                .Include(o => o.User)
                .Include(o => o.BookOrderDetails)
                    .ThenInclude(d => d.Book)
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set pagination data for the view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(orders);
        }


    }
}