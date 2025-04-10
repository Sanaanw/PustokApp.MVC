using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            AddToCookie();
             
            return View();
        }
        public IActionResult AddToSession()
        {
            HttpContext.Session.SetString("name", "Pb502");
            return Content("added");
        }
        public IActionResult GetDataFromSession()
        {
            var name = HttpContext.Session.GetString("name");
            return Content(name);
        }
        public IActionResult AddToCookie()
        {
            HttpContext.Response.Cookies.Append("name", "John");
            return Content("added");
        }
        public IActionResult GetDataFromCookie()
        {
            var name = HttpContext.Request.Cookies["name"];
            return Content(name);
        }
    }
}
