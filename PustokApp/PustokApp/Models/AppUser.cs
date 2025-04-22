using Microsoft.AspNetCore.Identity;
using PustokApp.Models.Home;

namespace PustokApp.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set;}
        public List<DbBasketItem> DbBasketItems { get; set; }
    }
}
