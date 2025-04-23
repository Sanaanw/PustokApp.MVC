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
    public class OrderController
        (
        PustokAppContext context,
        UserManager<AppUser> userManager
        )
        : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Member")]
        public IActionResult Checkout()
        {
            var user = userManager.Users
                .Include(x => x.DbBasketItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefault(x => x.UserName == User.Identity.Name);

            var checkoutVm = new CheckOutVm
            {
                CheckOutItems = user.DbBasketItems.Select(x => new CheckOutItemVm
                {
                    Name = x.Book.Title,
                    Count = x.Count,
                    Price = x.Book.Price
                }).ToList(),
                TotalPrice = user.DbBasketItems.Sum(x => x.Count * x.Book.Price),
            };

            return View(checkoutVm);
        }
        [HttpPost]
        [Authorize(Roles ="Member")]
        public IActionResult CheckOut(OrderVm orderVm)
        {

            var user = userManager.Users
               .Include(x => x.DbBasketItems)
               .ThenInclude(x => x.Book)
               .FirstOrDefault(x => x.UserName == User.Identity.Name);

            var checkoutVm = new CheckOutVm
            {
                CheckOutItems = user.DbBasketItems.Select(x => new CheckOutItemVm
                {
                    Name = x.Book.Title,
                    Count = x.Count,
                    Price = x.Book.Price
                }).ToList(),
                TotalPrice = user.DbBasketItems.Sum(x => x.Count * x.Book.Price),
                OrderVm = orderVm
            };
            if (!ModelState.IsValid)
                return View(checkoutVm);
            
            var order = new Order
            {
                TotalPrice=(int)user.DbBasketItems.Sum(x => x.Count * x.Book.Price),
                Address = orderVm.Address,
                Town = orderVm.Town,
                City = orderVm.City,
                ZipCode = orderVm.ZipCode,
                AppUserId = user.Id,
                OrderItems = user.DbBasketItems.Select(x => new OrderItem
                {
                    BookId = x.BookId,
                    Count = x.Count
                }).ToList(),
            };
            context.Order.Add(order);
            context.DbBasketItem.RemoveRange(user.DbBasketItems);
            context.SaveChanges();
            return RedirectToAction("Profile", "Account", new {tab="Order"});

        }
    }
}
