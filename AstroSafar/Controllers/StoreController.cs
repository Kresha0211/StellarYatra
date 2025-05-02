using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Razorpay.Api;

namespace AstroSafar.Controllers
{
    public class StoreController : Controller
    {
        private readonly SpaceLearningDBContext _context;
        public StoreController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        // Update your GetLoggedInUserId method:
        // Update your GetLoggedInUserId method:
        private int? GetLoggedInUserId()
        {
            var email = HttpContext.Session.GetString("UserEmail"); // Use the same key as in Explore()
            if (string.IsNullOrEmpty(email)) return null;

            var user = _context.Registrations.FirstOrDefault(u => u.Email == email);
            return user?.Id;
        }



        public IActionResult Explore()
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
            {
                TempData["RedirectAfterLogin"] = "ExploreBooks";
                return RedirectToAction("Login", "Account");
            }

            var books = _context.Books.ToList();

            // Fetch wishlist book ids for current user
            var wishlistBookIds = _context.Wishlists
                .Where(w => w.UserId == userId.Value)
                .Select(w => w.BookId)
                .ToList();

            ViewBag.WishlistBookIds = wishlistBookIds;

            return View(books);
        }


        public IActionResult Wishlist()
        {
            var userId = GetLoggedInUserId();

            var wishlist = _context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.UserId == userId.Value)
                .ToList();

            return View(wishlist);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToWishlist([FromBody] WishlistRequest request)
        {
            var userId = GetLoggedInUserId(); // however you're getting current user ID

            if (userId == null)
                return Json(new { success = false, message = "Please log in." });

            var exists = _context.Wishlists.Any(w => w.BookId == request.BookId && w.UserId == userId.Value);

            if (!exists)
            {
                _context.Wishlists.Add(new Wishlist
                {
                    BookId = request.BookId,
                    UserId = userId.Value
                });
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Already in wishlist." });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromWishlist([FromBody] WishlistRequest request)
        {
            var userId = GetLoggedInUserId();

            if (userId == null)
                return Json(new { success = false, message = "Please log in." });

            var wishlistItem = _context.Wishlists
                .FirstOrDefault(w => w.BookId == request.BookId && w.UserId == userId.Value);

            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                _context.SaveChanges();

                return Json(new { success = true, message = "Removed from wishlist." });
            }

            return Json(new { success = false, message = "Item not found in wishlist." });
        }

        // Add this model
        public class WishlistRequest
        {
            public int BookId { get; set; }
        }





        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Check if the book exists
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound(); // Book does not exist
            }

            // Check if already in cart
            var cartItem = _context.CartItems.FirstOrDefault(c => c.BookId == id && c.UserId == userId.Value);
            if (cartItem == null)
            {
                _context.CartItems.Add(new CartItem { BookId = id, UserId = userId.Value, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            _context.SaveChanges();
            return RedirectToAction("Cart");
        }



        public IActionResult Cart()
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId.Value).ToList();

            return View(cartItems);
        }


        [HttpPost]
        public IActionResult UpdateQuantity(int id, string action)
        {
            var cartItem = _context.CartItems.FirstOrDefault(c => c.CartItemId == id);
            if (cartItem == null) return NotFound();

            if (action == "increase")
                cartItem.Quantity++;
            else if (action == "decrease" && cartItem.Quantity > 1)
                cartItem.Quantity--;

            _context.SaveChanges();

            return Json(cartItem.Quantity); // ✅ return only quantity for JS update
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Cart");
        }



        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            var cartItemViewModels = cartItems.Select(c => new CartItemViewModel
            {
                ProductName = c.Book.Title,
                Quantity = c.Quantity,
                UnitPrice = c.Book.Price
            }).ToList();

            var subtotal = cartItems.Sum(c => c.Book.Price * c.Quantity);
            var gst = subtotal * 0.02M;
            var total = subtotal + gst;

            var model = new CheckoutViewModel
            {
                CartItems = cartItemViewModels,
                TotalAmount = total
            };

            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Checkout(CheckoutViewModel model)
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var cartItems = _context.CartItems
        //        .Include(c => c.Book)
        //        .Where(c => c.UserId == userId)
        //        .ToList();

        //    if (!cartItems.Any())
        //    {
        //        TempData["Error"] = "Cart is empty!";
        //        return RedirectToAction("Checkout");
        //    }

        //    var subtotal = cartItems.Sum(i => i.Book.Price * i.Quantity);
        //    var gst = subtotal * 0.02M;
        //    var total = subtotal + gst;

        //    if (model.PaymentMethod == "CashOnDelivery")
        //    {
        //        var order = new BookOrder
        //        {
        //            UserId = userId.Value,
        //            OrderDate = DateTime.Now,
        //            TotalAmount = total,
        //            FullName = model.FullName,
        //            AddressLine1 = model.AddressLine1,
        //            AddressLine2 = model.AddressLine2,
        //            City = model.City,
        //            State = model.State,
        //            ZipCode = model.ZipCode,
        //            PhoneNumber = model.PhoneNumber,
        //            PaymentMethod = model.PaymentMethod,
        //            BookOrderDetails = cartItems.Select(c => new BookOrderDetail
        //            {
        //                BookId = c.BookId,
        //                Quantity = c.Quantity,
        //                Price = c.Book.Price
        //            }).ToList()
        //        };

        //        _context.BookOrders.Add(order);
        //        _context.CartItems.RemoveRange(cartItems);
        //        _context.SaveChanges();

        //        return RedirectToAction("OrderSuccess");
        //    }
        //    else if (model.PaymentMethod == "OnlinePayment")
        //    {
        //        // Store shipping details in TempData or session (needed later when payment is successful)
        //        TempData["CheckoutData"] = JsonConvert.SerializeObject(model);
        //        TempData["TotalAmount"] = total;

        //        return View(model); // Razorpay will now trigger via JS
        //    }

        //    TempData["Error"] = "Invalid payment method selected.";
        //    return RedirectToAction("Checkout");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Cart is empty!";
                return RedirectToAction("Checkout");
            }

            var subtotal = cartItems.Sum(i => i.Book.Price * i.Quantity);
            var gst = subtotal * 0.02M;
            var total = subtotal + gst;

            if (model.PaymentMethod == "CashOnDelivery")
            {
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
                    PaymentMethod = model.PaymentMethod,
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

                return RedirectToAction("OrderSuccess");
            }
            else if (model.PaymentMethod == "OnlinePayment")
            {
                // Handle Razorpay payment completion here
                if (!string.IsNullOrEmpty(model.PaymentId))
                {
                    // Payment was successful, create the order
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
                        PaymentMethod = model.PaymentMethod,
                        PaymentId = model.PaymentId,
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

                    return RedirectToAction("OrderSuccess", new { paymentId = model.PaymentId });
                }
                else
                {
                    // Store shipping details in TempData (needed when returning to this page)
                    TempData["CheckoutData"] = JsonConvert.SerializeObject(model);
                    TempData["TotalAmount"] = total;

                    return View(model); // Razorpay will trigger via JS
                }
            }

            TempData["Error"] = "Invalid payment method selected.";
            return RedirectToAction("Checkout");
        }

        [HttpPost]
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
                PaymentMethod = model.PaymentMethod,
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

            return Ok();
        }

        //public IActionResult OrderSuccess(string paymentId)
        //{
        //    var order = _context.BookOrders
        //                .FirstOrDefault(o => o.PaymentId == paymentId);
        //    return View(order);
        //}

        public IActionResult OrderSuccess(string paymentId = null)
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            BookOrder order;

            if (!string.IsNullOrEmpty(paymentId))
            {
                // Find order by payment ID
                order = _context.BookOrders
                        .Include(o => o.BookOrderDetails)
                        .ThenInclude(d => d.Book)
                        .FirstOrDefault(o => o.PaymentId == paymentId);
            }
            else
            {
                // Find most recent COD order for this user
                order = _context.BookOrders
                        .Include(o => o.BookOrderDetails)
                        .ThenInclude(d => d.Book)
                        .Where(o => o.UserId == userId.Value)
                        .OrderByDescending(o => o.OrderDate)
                        .FirstOrDefault();
            }

            if (order == null)
            {
                TempData["Error"] = "Order not found";
                return RedirectToAction("Cart");
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult OrderSummary()
        {
            var userId = GetLoggedInUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // 1) Pull cart items from your DB
            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId.Value)
                .Select(c => new CartItemViewModel
                {
                    ProductName = c.Book.Title,
                    Quantity = c.Quantity,
                    UnitPrice = c.Book.Price
                    // TotalPrice is computed getter
                })
                .ToList();

            // 2) Pull shipping from Session
            var shipJson = HttpContext.Session.GetString("ShippingInfo");
            var shipping = JsonConvert.DeserializeObject<CheckoutViewModel>(shipJson);

            // 3) Compute totals and inject into CheckoutViewModel
            var subtotal = cartItems.Sum(i => i.TotalPrice);
            var gst = subtotal * 0.18M;
            var delivery = 0M;
            shipping.CartItems = cartItems;
            shipping.TotalAmount = subtotal + gst + delivery;

            return View("Checkout", shipping);
           
        }



        // STEP 5: ORDER CONFIRMATION
        [HttpGet]
        public IActionResult OrderConfirmation(string method)
        {
            ViewBag.PaymentMethod = method;
            return View();
        }

        [HttpPost]
        public IActionResult BuyNow(int bookId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Clear existing cart items for this user
            var existingItems = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(existingItems);
            _context.SaveChanges();

            // Add the book to cart with quantity 1
            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var cartItem = new CartItem
            {
                BookId = bookId,
                UserId = userId.Value,
                Quantity = 1
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return RedirectToAction("Checkout");
        }

    }




}