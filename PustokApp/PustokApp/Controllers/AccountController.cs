using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokApp.Models;
using PustokApp.ViewModels;
using System.Threading.Tasks;

namespace PustokApp.Controllers
{
    public class AccountController(
        UserManager<AppUser> userManager
        ) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm)
        {
            if (!ModelState.IsValid)
                return View(userRegisterVm);
            AppUser user= await userManager.FindByNameAsync(userRegisterVm.UserName);
            if (user != null)
                ModelState.AddModelError("UserName", "This username is already taken");
            user =new AppUser
            {
                FullName = userRegisterVm.FullName,
                UserName = userRegisterVm.UserName,
                Email = userRegisterVm.Email
            };
            var result = await userManager.CreateAsync(user, userRegisterVm.Password);
            if (!result.Succeeded)
            {
               foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return View();
        }
    }
}
