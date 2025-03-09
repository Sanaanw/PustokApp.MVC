using Microsoft.AspNetCore.Mvc;
using PustokApp.Data;

namespace PustokApp.Areas.Manage.Controllers
{
    public class BrandController(PustokAppContext context) : Controller
    {
        [Area("Manage")]
        public IActionResult Index()
        {
            var brands = context.Brand.ToList();
            return View(brands);
        }
    }
}
