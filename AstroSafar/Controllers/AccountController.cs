using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI;

namespace AstroSafar.Controllers
{
    public class AccountController : Controller
    {
        private readonly SpaceLearningDBContext _context;

        public AccountController(SpaceLearningDBContext context)
        {
            _context = context;
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
            var regis = _context.Registrations.FirstOrDefault(c => c.Email==email && c.Password == password);

            if (regis != null)
            {
                HttpContext.Session.SetInt32("RegisterId", regis.Id);
                HttpContext.Session.SetString("RegisterFN", regis.Firstname);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid Email or Password!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

