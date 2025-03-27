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
            {
                return View();
            }
             slider.Image= slider.Photo.SaveImage(env.WebRootPath, "assets/image/bg-images");
            context.Slider.Add(slider);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var slider = context.Slider.FirstOrDefault(b => b.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            var existSlider = context.Slider.FirstOrDefault(b => b.Id == slider.Id);
            if (existSlider == null) return NotFound();
            var oldImage = existSlider.Image;
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo is required");
                return View(slider);
            }
            if (slider.Photo != null)
            {

                existSlider.Image = slider.Photo.SaveImage(env.WebRootPath, "assets/image/bg-images");
                var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "assets/image/bg-images", oldImage);
                if (System.IO.File.Exists(deleteImagePath))
                {
                    System.IO.File.Delete(deleteImagePath);
                }
            }
            if (!ModelState.IsValid)
                return View(slider);
            existSlider.Title = slider.Title;
            existSlider.Desc = slider.Desc;
            existSlider.Order = slider.Order;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var existSlider = context.Slider.FirstOrDefault(b => b.Id == id);
            if (existSlider is null) return NotFound();
            var deleteImagePath = Path.Combine(Directory.GetCurrentDirectory(), "assets/image/bg-images", existSlider.Image);
            if (System.IO.File.Exists(deleteImagePath))
            {
                System.IO.File.Delete(deleteImagePath);
            }
            context.Slider.Remove(existSlider);
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
