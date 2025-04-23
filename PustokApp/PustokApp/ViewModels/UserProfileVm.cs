using PustokApp.Models.Home;

namespace PustokApp.ViewModels
{
    public class UserProfileVm
    {
        public UserUpdateProfileVm UserUpdateProfileVm { get; set; }
        public List<Order> Orders { get; set; }
    }
}
