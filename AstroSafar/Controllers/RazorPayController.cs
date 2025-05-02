using AstroSafar.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace AstroSafar.Controllers
{


    public class RazorpayController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SpaceLearningDBContext _context;
        public RazorpayController(IConfiguration configuration, SpaceLearningDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder(decimal amount)
        {
            try
            {
                // Convert amount to paise (integer)
                int amountInPaise = (int)(amount * 100);  // Convert to int after multiplying by 100

                // Initialize Razorpay client with keys
                var razorpay = new RazorpayClient("rzp_test_4UCmUidOmvrAtJ", "K3NACf1ILUSlwVUuyQLgC7bG");

                // Create order options
                var options = new Dictionary<string, object>
        {
            { "amount", amountInPaise },  // Use the integer value for amount
            { "currency", "INR" },
            { "receipt", "receipt#1" },
            { "payment_capture", 1 }  // Auto-capture payment
        };

                // Create Razorpay order
                var order = razorpay.Order.Create(options);
                return Json(new { success = true, orderId = order["id"].ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentSuccess([FromBody] RazorpaySuccessViewModel razorData)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized();

            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
                return BadRequest("Cart is empty");

            var modelJson = TempData["CheckoutData"]?.ToString();
            var amount = TempData["TotalAmount"]?.ToString();

            if (string.IsNullOrEmpty(modelJson) || string.IsNullOrEmpty(amount))
                return BadRequest("Checkout session expired");

            var model = JsonConvert.DeserializeObject<CheckoutViewModel>(modelJson);
            var total = Convert.ToDecimal(amount);

            var order = new BookOrder
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                FullName = model.FullName,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                PhoneNumber = model.PhoneNumber,
                PaymentMethod = "OnlinePayment",
                PaymentId = razorData.PaymentId, // Capture from Razorpay response
                BookOrderDetails = cartItems.Select(c => new BookOrderDetail
                {
                    BookId = c.BookId,
                    Quantity = c.Quantity,
                    Price = c.Book.Price
                }).ToList()
            };

            _context.BookOrders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            // Return OK with the payment ID so front-end can redirect
            return Ok(new { paymentId = razorData.PaymentId });
        }

        private bool IsValidPayment(string paymentId, string orderId, string signature)
        {
            var razorpayKeySecret = _configuration["rzp_test_4UCmUidOmvrAtJ"];

            // Create a string to compare
            var stringToVerify = $"{orderId}|{paymentId}";

            // Generate the HMAC SHA256 signature using your key secret
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(razorpayKeySecret)))
            {
                var computedSignature = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToVerify))).Replace("-", "").ToLower();
                Console.WriteLine($"Computed Signature: {computedSignature}");
                Console.WriteLine($"Received Signature: {signature}");
                return computedSignature == signature;
            }
        }

    }
}