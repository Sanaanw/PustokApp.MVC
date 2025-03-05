using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.ViewModels;

namespace PustokApp.Controllers;

public class HomeController(PustokAppContext context) : Controller
{

    public IActionResult Index()
    {
        HomeVm homeVm = new()
        {
            Sliders = context.Slider.ToList(),
            FeaturedBooks = context.Book
          .Where(x => x.IsFeatured)
          .Include(y => y.Author)
          .Include(z => z.BookImages.Where(bi => bi.Status != null))
          .ToList(),
            NewBooks = context.Book
          .Where(x => x.IsNew)
          .Include(y => y.Author)
          .Include(z => z.BookImages.Where(bi => bi.Status != null))
          .ToList(),
            DiscountBooks = context.Book
          .Where(x => x.DiscountPercent > 0)
          .Include(y => y.Author)
          .Include(z => z.BookImages.Where(bi => bi.Status != null))
          .ToList(),
            Features = context.Feature.ToList()
        };
        return View(homeVm);
    }

}
