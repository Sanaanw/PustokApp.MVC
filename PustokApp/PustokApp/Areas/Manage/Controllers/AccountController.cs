using Microsoft.AspNetCore.Authorization;
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
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager
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

            await userManager.AddToRoleAsync(User, "Admin");

            return Json(result);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm,string returnUrl)
        {
            if (!ModelState.IsValid) return View();

            var user = await userManager.FindByNameAsync(adminLoginVm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            if (!await userManager.IsInRoleAsync(user, "SuperAdmin") && ! await userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", "You are not allowed to login");
                return View();
            }

            var result = await userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if (!result)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            await signInManager.SignInAsync(user, true);

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Dashboard");

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");   
        }
        [Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<IActionResult> UserProfile()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return Json(user);
        }
        public async Task<IActionResult> CreateRole()
        {

            await roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            await roleManager.CreateAsync(new IdentityRole { Name="Admin"});
            await roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            return Content("Roles Created");
        }
    }
}
