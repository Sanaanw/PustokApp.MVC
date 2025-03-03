using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models.Home;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    public class BookController(PustokAppContext context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 

        public IActionResult Detail(int? Id)
        {
            if (Id == null)
                return NotFound();
            var ExistBook = context.Book
                .Include(b => b.Brand)
                .Include(a => a.Author)
                .Include(bi => bi.BookImages)
                .Include(Bt => Bt.BookTags)
                .ThenInclude(t => t.Tag)
                .FirstOrDefault(y => y.Id == Id);
            if (ExistBook == null)
                return NotFound();

            BookDetailVm bookDetailVm = new()
            {
                Book = ExistBook,
                ReleatedBook = context.Book
                .Include(b => b.Brand)
                .Include(a => a.Author)
                .Include(bi => bi.BookImages)
                .Where(x=>x.AuthorId==ExistBook.AuthorId)
                .ToList()
            };

            return View(bookDetailVm);
        }
    }
}
