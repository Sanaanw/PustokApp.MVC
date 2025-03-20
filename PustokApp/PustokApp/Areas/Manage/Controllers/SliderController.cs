using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PustokApp.Data;
using PustokApp.Helpers;
using PustokApp.Models.Home;

namespace PustokApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly PustokAppContext context;
        private readonly JwtServiceOption jwtServiceOption;
        private readonly IWebHostEnvironment env;
        public SliderController(PustokAppContext _context, IOptions<JwtServiceOption> options, IWebHostEnvironment _env)
        {
            context = _context;
            jwtServiceOption = options.Value;
            env = _env;
        }
        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = context.Slider.AsQueryable();
            PaginatedList<Slider> paginatedList = PaginatedList<Slider>.Create(query, page, take);
            return View(paginatedList);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var slider = context.Brand.FirstOrDefault(b => b.Id == id);
            if (slider == null) return NotFound();
            context.Brand.Remove(slider);
            context.SaveChanges();
            return Ok();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo is required");
                return View(); 
            }
            if (!ModelState.IsValid)
                return View();
            if(!slider.Photo.CheckType(new string[] {"image/jpeg","image/png"}))
            {
                ModelState.AddModelError("Photo", "Photo type isn't appropriate");
                return View();
            }
            if (!slider.Photo.CheckSize(2*1024*1024))
            {
                ModelState.AddModelError("Photo", "Photo size can't be less than 2MB");
                return View();
            }
             
             slider.Image= slider.Photo.SaveImage(env.WebRootPath, "assets/image/bg-images");
            context.Slider.Add(slider);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult ReadData()
        {
            var data1 = jwtServiceOption.Key;
            var data2 = jwtServiceOption.Issuer;
            return Json(new
            {
                key = data1,
                issuer = data2
            });
        }
    }
}
