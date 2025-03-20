using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokApp.Models.Home
{
    public class Slider : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        public string Image { get; set; }
        [Required]
        public int Order { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
