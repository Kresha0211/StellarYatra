using AstroSafar.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstroSafar.Controllers
{

    public class BooksController : Controller
    {
        private readonly SpaceLearningDBContext _context;

        public BooksController(SpaceLearningDBContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, int pageSize = 5)
        {
            var allBooks = _context.Books.ToList();
            int total = allBooks.Count();
            int totalPages = (int)Math.Ceiling((double)total / pageSize);

            var paginatedBooks = allBooks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedBooks);
        }
        public IActionResult Create() => View();

        //[HttpPost]
        //public IActionResult Create(Book book)
        //{
        //    _context.Books.Add(book);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public IActionResult Create(Book book, IFormFile ImageUrl)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                // Generate a unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUrl.FileName);

                // Path to save image: wwwroot/images/books
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");

                // Create directory if it doesn't exist
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var fullPath = Path.Combine(savePath, fileName);

                // Save the image
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }

                // Save relative path to database
                book.ImageUrl = "/images/books/" + fileName;
            }

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //public IActionResult Edit(int id)
        //{
        //    var book = _context.Books.Find(id);
        //    return View(book);
        //}

        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book); // ✅ this sends the model to your view
        }


        //[HttpPost]
        //public IActionResult Edit(Book book)
        //{
        //    _context.Books.Update(book);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Edit(Book book, IFormFile ImageUrl)
        {
            var existingBook = _context.Books.Find(book.BookId);

            if (existingBook == null)
                return NotFound();

            // Update the fields
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;

            // Handle image upload if a new image is selected
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var fileName = Path.GetFileName(ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageUrl.CopyTo(stream);
                }

                existingBook.ImageUrl = "/Images/" + fileName; // Assuming this is the correct format used in your views
            }

            _context.Books.Update(existingBook);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }




        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books.Find(id);
            return View(book);
        }
    }

}