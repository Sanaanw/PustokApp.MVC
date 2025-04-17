using System.ComponentModel.DataAnnotations;

namespace PustokApp.ViewModels
{
    public class ResetPasswordVm
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPasword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPasword", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
