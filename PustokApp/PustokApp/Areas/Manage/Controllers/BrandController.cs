using Microsoft.AspNetCore.Mvc;
using PustokApp.Data;

namespace PustokApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController(PustokAppContext context) : Controller
    {
        public IActionResult Index()
        {
            var brands = context.Brand.ToList();
            return View(brands);
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
    }
}
