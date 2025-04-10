using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Helpers;
using PustokApp.Models.Home;

namespace PustokApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController(PustokAppContext context) : Controller
    {
        public IActionResult Index(int page=1,int take=2)
        {
            var query = context.Brand.Include(x=>x.Books);
            PaginatedList<Brand> paginatedList = PaginatedList<Brand>.Create(query, page, take);
            return View(paginatedList);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var brand = context.Brand.FirstOrDefault(b => b.Id == id);
            if (brand == null) return NotFound();
            context.Brand.Remove(brand);
            context.SaveChanges();
           return Ok();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
                return View();
            if (context.Brand.Any(x => x.Name == brand.Name))
            {
                ModelState.AddModelError("Name", "This brand already exist");
                return View();
            }
            context.Brand.Add(brand);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var brand = context.Brand.FirstOrDefault(b => b.Id == id);
            if (brand == null) return NotFound();
            return View(brand);
        }
        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid)
                return View();
            var ExistBrand = context.Brand.FirstOrDefault(b => b.Id == brand.Id);
            if (ExistBrand == null) return NotFound();
            if (ExistBrand.Name != brand.Name && context.Brand.Any(x=>x.Name==brand.Name && x.Id!=ExistBrand.Id))
            {
                ModelState.AddModelError("Name", "This brand already exist");
                return View();
            }
            ExistBrand.Name = brand.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Detail(int? id)
        {
            if (id == null) return NotFound();
            var brand = context.Brand.FirstOrDefault(b => b.Id == id);
            if (brand == null) return NotFound();
            return View(brand);
        }
    }
}
