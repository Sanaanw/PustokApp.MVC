using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.Models.Home;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    public class BasketController(
        PustokAppContext context,
        UserManager<AppUser> userManager
        ) : Controller
    {
        public IActionResult Index()
        {
            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVm> basketItemVmList;
            if (basket != null)
                basketItemVmList = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            else
            {
                basketItemVmList = new();
               
            }
            foreach (var item in basketItemVmList)
            {
                var book = context.Book
                    .Include(b => b.BookImages)
                    .FirstOrDefault(b => b.Id == item.BookId);
                item.MainImage = book.BookImages
                    .FirstOrDefault(bi => bi.Status == true).Name;
                item.Price = book.Price;
                item.Name = book.Title;

            }
            return View(basketItemVmList);
        }
        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
                return NotFound();
            var book = context.Book
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            List<BasketItemVm> baskets;
            var basket = HttpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                baskets = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            }
            else
            {
                baskets = new();
            }

            var existBook = baskets.FirstOrDefault(b => b.BookId == id);
            if (existBook != null)
            {
                existBook.Count++;
            }
            else
            {
                BasketItemVm basketItem = new()
                {
                    BookId = book.Id,
                    Name = book.Title,
                    MainImage = book.BookImages.FirstOrDefault(bi => bi.Status == true).Name,
                    Price = book.Price,
                    Count = 1
                };
                baskets.Add(basketItem);
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = userManager.Users
                    .Include(b => b.DbBasketItems)
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                var existUserBasketItem = user.DbBasketItems
                    .FirstOrDefault(b => b.BookId == id);
                if (existUserBasketItem != null)
                {
                    existUserBasketItem.Count++;
                }
                else
                {
                    user.DbBasketItems.Add(new DbBasketItem
                    {
                        BookId = book.Id,
                        Count = 1,
                        AppUserId = user.Id
                    });
                }
                context.SaveChanges();
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));

            return PartialView("_BasketPartial", baskets); 
        }
    }
}
