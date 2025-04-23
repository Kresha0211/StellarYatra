using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

using AstroSafar.Models;
using Microsoft.Extensions.Options;

namespace AstroSafar.Controllers
{

    public class PaymentController : Controller
    {
        private readonly SpaceLearningDBContext _context;
        private readonly RazorpaySettings _razorpaySettings;

        private readonly IConfiguration _configuration;

        public PaymentController(SpaceLearningDBContext context, IConfiguration configuration, IOptions<RazorpaySettings> options)
        {

            _context = context;
            _configuration = configuration;
            _razorpaySettings = options.Value;

        }



        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder(int courseId, decimal amount)
        {
            try
            {
                if (courseId <= 0 || amount <= 0)
                {
                    return BadRequest(new { error = "Invalid Course ID or Amount" });
                }

                // Verify enrollment exists first
                int enrollmentId = GetEnrollmentId(courseId);
                if (enrollmentId <= 0)
                {
                    return BadRequest(new { error = "No valid enrollment found for this course" });
                }

                string key = _configuration["Razorpay:Key"];
                string secret = _configuration["Razorpay:Secret"];
                RazorpayClient client = new RazorpayClient(key, secret);

                Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", (int)(amount * 100) }, // Convert to paise
            { "currency", "INR" },
            { "payment_capture", 1 }
        };

                Order order = client.Order.Create(options);

                return Ok(new
                {
                    orderId = order["id"].ToString(),
                    enrollmentId = enrollmentId // Use the retrieved enrollment ID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Payment error: " + ex.Message });
            }
        }

        private int GetEnrollmentId(int courseId)
        {
            var enrollment = _context.enrollments
                .Where(e => e.CourseId == courseId)
                .OrderByDescending(e => e.Id)  // Sort by ID to get newest
                .FirstOrDefault();

            if (enrollment == null)
            {
                throw new Exception("No enrollment found for this course");
            }

            return enrollment.Id;
        }

        public IActionResult Payment(int enrollmentId)
        {
            // Check if payment already exists for this enrollment
            var existingPayment = _context.Transactions
                .FirstOrDefault(t => t.EnrollmentId == enrollmentId && t.Status == "Success");

            if (existingPayment != null)
            {
                // Payment already exists, redirect to certificate page
                TempData["Message"] = "Your payment is already completed. Here is your certificate.";
                return RedirectToAction("CertificateGenerated", "Certificate", new { enrollmentId });
            }

            // If no payment exists, proceed with payment creation
            var options = new Dictionary<string, object>
    {
        { "amount", 49900 }, // Amount in paise (499 INR)
        { "currency", "INR" },
        { "receipt", "receipt_" + enrollmentId },
        { "payment_capture", "1" }
    };

            RazorpayClient client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);
            Order order = client.Order.Create(options);

            ViewBag.OrderId = order["id"];
            ViewBag.Key = _razorpaySettings.Key;
            ViewBag.Amount = options["amount"];
            ViewBag.EnrollmentId = enrollmentId;

            return View();
        }

        [HttpPost]
        public IActionResult PaymentCallback(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int enrollmentId)
        {
            try
            {
                // First check if a successful payment already exists
                var existingPayment = _context.Transactions
                    .FirstOrDefault(t => t.EnrollmentId == enrollmentId && t.Status == "Success");

                if (existingPayment != null)
                {
                    // Payment already exists, redirect to certificate page
                    TempData["Message"] = "Your payment is already completed. Here is your certificate.";
                    return RedirectToAction("CertificateGenerated", "Certificate", new { enrollmentId });
                }

                // ✅ (Optional) Verify Razorpay signature here using their SDK

                // ✅ Step 1: Save payment record
                var payment = new Transaction
                {
                    EnrollmentId = enrollmentId,
                    Amount = 499, // ₹499
                    RazorpayPaymentId = razorpay_payment_id,
                    RazorpayOrderId = razorpay_order_id,
                    RazorpaySignature = razorpay_signature,
                    PaymentDate = DateTime.Now,
                    Currency = "INR",
                    Status = "Success" // Set status to Success
                };

                _context.Transactions.Add(payment);
                _context.SaveChanges();

                // ✅ Step 2: Check if certificate already exists
                var existing = _context.Certificates.FirstOrDefault(c => c.EnrollmentId == enrollmentId);
                if (existing == null)
                {
                    // 🔁 Step 3: Generate a unique certificate number
                    int lastId = _context.Certificates.Any()
                        ? _context.Certificates.Max(c => c.Id)
                        : 0;

                    string certNumber = $"ASTRO-{DateTime.Now.Year}-{(lastId + 1).ToString("D4")}";

                    // 🟢 Step 4: Create new certificate
                    var certificate = new Certificate
                    {
                        EnrollmentId = enrollmentId,
                        IssuedOn = DateTime.Now,
                        CertificateNumber = certNumber,
                        IsDownloaded = false
                    };

                    _context.Certificates.Add(certificate);
                    _context.SaveChanges();
                }

                // ✅ Step 5: Redirect to Certificate View
                return RedirectToAction("CertificateGenerated", "Certificate", new { enrollmentId });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel
                {
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        private void SavePaymentDetails(Transaction payment)
        {
            _context.Transactions.Add(payment);
            _context.SaveChanges();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failure()
        {
            return View();
        }
    }
}