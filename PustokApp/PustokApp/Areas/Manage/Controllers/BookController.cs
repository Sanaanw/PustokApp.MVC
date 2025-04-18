using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PustokApp.Data;
using PustokApp.Helpers;
using PustokApp.Models.Home;
using PustokApp.Services;
using PustokApp.Settings;

namespace PustokApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly PustokAppContext context;
        private readonly EmailService emailService;
        private readonly IOptions<EmailSetting> emailOptions;
        public BookController(
            PustokAppContext _context,
            IWebHostEnvironment _env,
            EmailService _emailService,
            IOptions<EmailSetting> _emailOptions 
            )
        {
            context = _context;
            env = _env;
            emailService = _emailService;
            emailOptions = _emailOptions;
        }
        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = context.Book
                .Include(x => x.Author)
                .Include(x => x.Brand)
                .Include(x => x.BookImages)
                .AsQueryable();
            PaginatedList<Book> data = PaginatedList<Book>.Create(query, page, take);
            return View(data);
        }

        public IActionResult Delete(int? id)
        {
            var existBook = context.Book
                .Include(x => x.BookImages)
                .FirstOrDefault(b => b.Id == id);
            if (existBook is null) return NotFound();
            foreach (var image in existBook.BookImages)
            {
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/image/products", image.Name);
                if (System.IO.File.Exists(deleteImagePath))
                {
                    System.IO.File.Delete(deleteImagePath);
                }
            }
            context.Book.Remove(existBook);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            ViewBag.Authors = new SelectList(context.Author.ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(context.Brand.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = new SelectList(context.Author.ToList(), "Id", "Name");
            ViewBag.Brands = new SelectList(context.Brand.ToList(), "Id", "Name");

            if (!context.Author.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author is required");
                return View();
            }
            if (!context.Brand.Any(x => x.Id == book.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand is required");
                return View();
            }
            if (book.Files != null)
            {
                foreach (var file in book.Files)
                {
                    BookImage bookImage = new();
                    bookImage.BookId = book.Id;
                    bookImage.Name = file.SaveImage(env.WebRootPath, "assets/image/products");
                    book.BookImages.Add(bookImage);
                }
            }
            else
            {
                ModelState.AddModelError("Files", "Images are required");
                return View();
            }
            if (book.MainFile != null)
            {
                BookImage bookImage = new();
                bookImage.Status = true;
                bookImage.Name = book.MainFile.SaveImage(env.WebRootPath, "assets/image/products");
                book.BookImages.Add(bookImage);
            }
            else
            {
                ModelState.AddModelError("MainFile", "Main image is required");
                return View();
            }
            if (book.HoverFile != null)
            {
                BookImage bookImage = new();
                bookImage.Status = false;
                bookImage.Name = book.HoverFile.SaveImage(env.WebRootPath, "assets/image/products");
                book.BookImages.Add(bookImage);
            }
            else
            {
                ModelState.AddModelError("HoverFile", "Hover image is required");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }


            context.Book.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
