using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace AstroSafar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SpaceLearningDBContext _context;
        private bool abcd = false;
         public AccountController(SpaceLearningDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
      
        [HttpGet]
        public IActionResult CheckUserAuthentication()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            return Json(new { isAuthenticated });
        }
        // Profile
        //public IActionResult Profile()
        //{
        //    int? userId = HttpContext.Session.GetInt32("CustomerId");

        //    if (userId == null)
        //    {
        //        Debug.WriteLine("User is not logged in. Redirecting to login...");
        //        return RedirectToAction("Login");
        //    }

        //    var user = _context.Registrations.Find(userId);

        //    if (user == null)
        //    {
        //        Debug.WriteLine($"User with ID {userId} not found in the database.");
        //        return NotFound();
        //    }

        //    Debug.WriteLine($"Loading profile for user: {user.Firstname} {user.Lastname}");
        //    return View(user);
        //}

        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("CustomerId");

            if (userId == null)
            {
                Debug.WriteLine("User is not logged in. Redirecting to login...");
                return RedirectToAction("Login");
            }

            var user = _context.Registrations.Find(userId);
            if (user == null)
            {
                Debug.WriteLine($"User with ID {userId} not found.");
                return NotFound();
            }

            return View(user);
        }


        //[HttpGet]
        //public IActionResult EditProfile()
        //{
        //    int? userId = HttpContext.Session.GetInt32("CustomerId");

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var user = _context.Registrations.Find(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        [HttpGet]
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("CustomerId");

            if (userId == null)
            {
                Debug.WriteLine("Session expired or user not logged in. Redirecting to login.");
                return RedirectToAction("Login");
            }

            var user = _context.Registrations.Find(userId);
            if (user == null)
            {
                Debug.WriteLine($"User with ID {userId} not found.");
                return NotFound();
            }

            return View(user);
        }


        //[HttpPost]
        //public async Task<IActionResult> Edit(Registration model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    int? userId = HttpContext.Session.GetInt32("CustomerId");

        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    var user = await _context.Registrations.FindAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update user details
        //    user.Firstname = model.Firstname;
        //    user.Lastname = model.Lastname;
        //    user.Email = model.Email;
        //    user.Phone = model.Phone;
        //    user.Password = model.Password;
        //    user.DateOfBirth = model.DateOfBirth;// Consider hashing the password

        //    _context.Registrations.Update(user);
        //    await _context.SaveChangesAsync();

        //    ViewBag.SuccessMessage = "Profile updated successfully!";
        //    return RedirectToAction("Profile");
        //}
        [HttpPost]
        public async Task<IActionResult> EditProfile(Registration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userId = HttpContext.Session.GetInt32("CustomerId");
            if (userId == null)
            {
                Debug.WriteLine("Session expired. Redirecting to login.");
                return RedirectToAction("Login");
            }

            var user = await _context.Registrations.FindAsync(userId);
            if (user == null)
            {
                Debug.WriteLine($"User with ID {userId} not found.");
                return NotFound();
            }

            // Update user details
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Email = model.Email;
            user.Phone = model.Phone;

            // 🚨 SECURITY: Do NOT store plain-text passwords!
            // Ideally, update password only if changed
            if (!string.IsNullOrEmpty(model.Password) && model.Password != user.Password)
            {
                user.Password = model.Password; // Consider hashing!
            }

            user.DateOfBirth = model.DateOfBirth;

            _context.Registrations.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }



        // GET: Registration Page
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register Customer
        [HttpPost]
        public async Task<IActionResult> Register(Registration registration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Registrations.Add(registration);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Login", "Account");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.ValidationErrors = errors;
            }

            return View(registration);
        }

        [HttpGet]
        public IActionResult Login(bool isClickedFromExporePage)
        {
           
            TempData["boolVaribale"] = isClickedFromExporePage;
            return View();
        }

        [HttpPost]
       
        [HttpPost]
        public IActionResult Login(string email, string password, string returnUrl = null)
        {
            bool abc = (bool)(TempData["boolVaribale"]);


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Email and password are required.";
                return View();
            }

            var customer = _context.Registrations.FirstOrDefault(c => c.Email == email && c.Password == password);
            if (customer == null)
            {
                ViewBag.Message = "Email not found! Please check your email or register.";
                return View();
            }

            if (customer.Password != password)
            {
                ViewBag.Message = "Incorrect password! Please try again.";
                return View();
            }

            // Set session data
            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("CustomerName", customer.Firstname);
         


            // Redirect to returnUrl if available, otherwise go to Dashboard
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (abc)
            {
                return RedirectToAction("EducationLevels", "Home");

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

