using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;

namespace AstroSafar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SpaceLearningDBContext _context;

         public AccountController(SpaceLearningDBContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //[HttpGet]
        //public IActionResult CheckUserAuthentication()
        //{
        //    Console.WriteLine("🔹 Checking User Authentication...");
        //    bool isAuthenticated = User.Identity.IsAuthenticated;
        //    Console.WriteLine($"✅ User Authenticated: {isAuthenticated}");
        //    return Json(isAuthenticated);
        //}

        //[HttpGet]
        //public IActionResult CheckUserAuthentication()
        //{
        //    bool isAuthenticated = User.Identity.IsAuthenticated;
        //    return Json(new { isAuthenticated }); // Send JSON response
        //}

        [HttpGet]
        public IActionResult CheckUserAuthentication()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            return Json(new { isAuthenticated });
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


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var regis = _context.Registrations.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (regis != null)
            {
                HttpContext.Session.SetInt32("RegisterId", regis.Id);
                HttpContext.Session.SetString("RegisterFN", regis.Firstname);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid Email or Password!";
            return View();
        }

<<<<<<< HEAD

=======
        
>>>>>>> 9f11d452a012207b723c953258bd01175e53f43d
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

