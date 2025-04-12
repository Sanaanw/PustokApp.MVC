using System.ComponentModel.DataAnnotations;

namespace PustokApp.ViewModels
{
    public class UserLoginVm
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
