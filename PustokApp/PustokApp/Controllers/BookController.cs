using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.Models.Home;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    public class BookController(PustokAppContext context,
        UserManager<AppUser> userManager
        ) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? Id)
        {
            if (Id == null)
                return NotFound();
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var vm = GetBookDetailVm((int)Id, user.Id);
                if (vm.Book == null)
                    return NotFound();
                return View(vm);
            }
            else
            {
                var vm = GetBookDetailVm((int)Id);
                if (vm.Book == null)
                    return NotFound();
                return View(vm);
            }
        }

        [HttpPost]
        [Authorize(Roles ="Member")]
        public async Task<IActionResult>AddComment(BookComment bookComment)
        {

            if (!context.Book.Any(bc => bc.Id == bookComment.BookId))
                               return NotFound();
            var user= await userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Detail", "Book", new { returnUrl = Url.Action("Detail", "Book", new { id = bookComment.BookId }) });
            if (!ModelState.IsValid)
            {
                var vm = GetBookDetailVm(bookComment.BookId, user.Id);
                vm.BookComment = bookComment;
                return View("Detail",vm);
            }


            bookComment.AppUserId = userManager.GetUserId(User);
            context.BookComment.Add(bookComment);
            await context.SaveChangesAsync();

            return RedirectToAction("Detail", "Book", new { returnUrl = Url.Action("Detail", "Book", new { id = bookComment.BookId }) });

        }

        public IActionResult BookModal(int? Id)
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
            return PartialView("_ModelBookPartial", ExistBook);
        }

        private BookDetailVm GetBookDetailVm(int bookId, string userId = null)
        {
            var ExistBook = context.Book
               .Include(b => b.Brand)
               .Include(a => a.Author)
               .Include(bi => bi.BookImages)
               .Include(b => b.BookComments)
               .ThenInclude(bc => bc.AppUser)
               .Include(Bt => Bt.BookTags)
               .ThenInclude(t => t.Tag)
               .FirstOrDefault(y => y.Id == bookId);

            if (ExistBook == null)
                return null;

            BookDetailVm bookDetailVm = new()
            {
                Book = ExistBook,
                ReleatedBook = context.Book
                    .Include(b => b.Brand)
                    .Include(a => a.Author)
                    .Include(b => b.BookComments)
                    .Include(bi => bi.BookImages)
                    .Where(x => x.AuthorId == ExistBook.AuthorId && x.Id != bookId)
                    .ToList()
            };

            if (userId != null)
            {
                var user = userManager.FindByNameAsync(User.Identity.Name).Result;

                if (user != null)
                {
                    bookDetailVm.HasComment = context.BookComment
                        .Any(bc => bc.AppUserId == user.Id && bc.BookId == bookId && bc.Status != CommentStatus.Rejected);
                }
            }

            bookDetailVm.TotalComments = context.BookComment
                .Count(bc => bc.BookId == bookId);

            var rates = context.BookComment
                .Where(bc => bc.BookId == bookId)
                .Select(bc => (decimal?)bc.Rate)
                .ToList();

            bookDetailVm.AvgRate = rates.Any() ? rates.Average() ?? 0 : 0;

            return bookDetailVm;
        }

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> DeleteComment(int? Id)
        {
            if (Id == null)
                return NotFound();
            var existComment = context.BookComment
                 .Include(b => b.AppUser)
                .FirstOrDefault(b => b.Id == Id);
            if (existComment == null)
                return NotFound();
            if (existComment.AppUserId != userManager.GetUserId(User))
                return NotFound();
            context.BookComment.Remove(existComment);
            await context.SaveChangesAsync();
            return RedirectToAction("Detail", "Book", new { id = existComment.BookId });
        }

    }
}
