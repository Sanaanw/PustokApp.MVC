using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.Models.Home;
using PustokApp.ViewModels;

namespace PustokApp.Services
{
    public class LayoutService(PustokAppContext context,
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager
        )
    {
        public Dictionary<string,string> GetSettings()
        {
            return context.Setting
                .ToDictionary(s => s.Key, s => s.Value);
        }
        public List<Brand> GetBrands()
        {
            return context.Brand.ToList();
        }
        public List<BasketItemVm> GetBasket()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var basket = httpContext.Request.Cookies["basket"];
            var basketList = new List<BasketItemVm>();
            if (basket != null)
                basketList = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var user= userManager.Users
                    .Include(u => u.DbBasketItems)
                    .ThenInclude(b => b.Book)
                    .ThenInclude(b => b.BookImages)
                    .FirstOrDefault(u => u.UserName == httpContext.User.Identity.Name);
                foreach (var item in user.DbBasketItems)
                {
                    if (!basketList.Any(bi=>bi.BookId==item.BookId))
                    {
                        basketList.Add(new BasketItemVm
                        {
                            BookId = item.BookId,
                            Name = item.Book.Title,
                            MainImage = item.Book.BookImages.FirstOrDefault(x => x.Status == true).Name,
                            Price = item.Book.Price,
                            Count = item.Count
                        });
                    }
                }
                httpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketList));
            }
            foreach (var item in basketList)
            {
                var book = context.Book
                    .Include(b => b.BookImages)
                    .FirstOrDefault(b => b.Id == item.BookId);
                item.MainImage = book.BookImages
                    .FirstOrDefault(bi => bi.Status == true).Name;
                item.Price = book.Price;
                item.Name = book.Title;

            } 
            return basketList;
        }
    }
}
