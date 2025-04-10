using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokApp.Areas.Manage.ViewModels;
using PustokApp.Models;
using System.Threading.Tasks;

namespace PustokApp.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
        ) : Controller
    {
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser User = new()
            {
                UserName = "admin",
                FullName = "Admin",
                Email = "admin@gmail.com"
            };
            var result= await userManager.CreateAsync(User, "_Admin123");
            return Json(result);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm)
        {
            if (!ModelState.IsValid) return View();

            var user = await userManager.FindByNameAsync(adminLoginVm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            var result = await userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if (!result)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            await signInManager.SignInAsync(user, true);

            return RedirectToAction("Index", "Dashboard");
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");   
        }
        public async Task<IActionResult> UserProfile()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return Json(user);
        }
    }
}
