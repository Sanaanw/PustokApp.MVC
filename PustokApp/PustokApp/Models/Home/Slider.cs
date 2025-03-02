using System.ComponentModel.DataAnnotations;

namespace PustokApp.Models.Home
{
    public class Slider:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
