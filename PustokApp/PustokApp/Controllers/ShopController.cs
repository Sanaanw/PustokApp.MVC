using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.ViewModels;

namespace PustokApp.Controllers
{
    public class ShopController
        (
            PustokAppContext context
        )
        : Controller
    {
        public IActionResult Index(
            int? brandId = null, List<int> authorIds = null, List<int> tagIds = null,
            string sort = "AtoZ", int? minPrice = null, int? maxPrice = null
            )
        {
            ShopVm shopVm = new ShopVm
            {
                Authors = context.Author.ToList(),
                Tags = context.Tag.ToList(),
                Brands = context.Brand.ToList(),
            };
            var query = context.Book
                .Include(b => b.Author)
                .Include(b => b.Brand)
                .Include(b => b.BookImages.Where(x => x.Status != null))
                .AsQueryable();
            //Min and Max Price
            var minPriceDb = context.Book.Min(b => b.Price);
            var maxPriceDb = context.Book.Max(b => b.Price);

            if (brandId != null)
                query = query.Where(b => b.BrandId == brandId);

            if (authorIds != null && authorIds.Count > 0)
                query = query.Where(b => authorIds.Contains(b.AuthorId));

            if (tagIds != null && tagIds.Count > 0)
                query = query.Where(b => b.BookTags.Any(bt => tagIds.Contains(bt.TagId)));

            if (minPrice != null && maxPrice != null)
                query = query.Where(b => b.Price-b.Price*b.DiscountPercent >= minPrice && b.Price <= maxPrice);

            switch (sort)
            {
                case "ZtoA":
                    query = query.OrderByDescending(b => b.Title);
                    break;
                case "PriceAsc":
                    query = query.OrderBy(b => b.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(b => b.Price);
                    break;
                default:
                    query = query.OrderBy(b => b.Title);
                    break;
            }

            ViewBag.BrandId = brandId;
            ViewBag.AuthorIds = authorIds;
            ViewBag.TagIds = tagIds;
            ViewBag.Sort = sort;
            ViewBag.MinPrice = minPriceDb;
            ViewBag.MaxPrice = maxPriceDb;

            ViewBag.SelectedMinPrice = minPrice ?? minPriceDb;
            ViewBag.SelectedMaxPrice = maxPrice ?? maxPriceDb;


            shopVm.Books = query.ToList();
            return View(shopVm);
        }

    }
}
