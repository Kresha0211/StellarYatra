using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Azure.Core;
using System.Globalization;

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
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            int? userId = HttpContext.Session.GetInt32("CustomerId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Registrations.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(Registration model)
        {
            var user = _context.Registrations.Find(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.DateOfBirth = model.DateOfBirth;
            user.Password = model.Password;

            HttpContext.Session.SetString("CustomerName", $"{user.Firstname}");

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Profile updated successfully!";

            return RedirectToAction("Profile", new { id = user.Id }); // Redirect to profile page
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
        public IActionResult Login(string email, string password)
        {
            // validate user...
            var customer = _context.Registrations.FirstOrDefault(c => c.Email == email);
            if (customer == null || customer.Password != password)
            {
                ViewBag.Message = "Invalid credentials";
                return View();
            }

            // ✅ Set session
            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("UserEmail", customer.Email);
            HttpContext.Session.SetInt32("UserId", customer.Id);
            HttpContext.Session.SetString("CustomerName", customer.Firstname); // or user.Username



            // ✅ Redirect properly
            if (TempData["RedirectAfterLogin"] != null)
            {
                string redirectTo = TempData["RedirectAfterLogin"].ToString();

                if (redirectTo == "ExploreBooks")
                    return RedirectToAction("Explore", "Store");

                if (redirectTo == "EducationLevels")
                    return RedirectToAction("EducationLevels", "Home");
            }

            return RedirectToAction("Index", "Home");
        }



        //[HttpPost]
        //public IActionResult Login(string email, string password, string returnUrl = null)
        //{
        //    // Check if TempData has a value before casting
        //    bool abc = TempData["boolVaribale"] != null && (bool)TempData["boolVaribale"];

        //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        //    {
        //        ViewBag.Message = "Email and password are required.";
        //        return View();
        //    }

        //    var customer = _context.Registrations.FirstOrDefault(c => c.Email == email);
        //    if (customer == null)
        //    {
        //        ViewBag.Message = "Email not found! Please check your email or register.";
        //        return View();
        //    }

        //    if (customer.Password != password)
        //    {
        //        ViewBag.Message = "Incorrect password! Please try again.";
        //        return View();
        //    }

        //    // Set session data
        //    HttpContext.Session.SetInt32("CustomerId", customer.Id);
        //    HttpContext.Session.SetString("CustomerName", customer.Firstname);
        //    HttpContext.Session.SetString("UserEmail", customer.Email);

        //    // Redirect to returnUrl if available and valid
        //    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //    {
        //        return Redirect(returnUrl);
        //    }

        //    // If TempData flag is true, redirect to EducationLevels, otherwise to Home
        //    return abc ? RedirectToAction("EducationLevels", "Home") : RedirectToAction("Index", "Home");
        //}



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}