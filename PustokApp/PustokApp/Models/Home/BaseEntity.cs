using System.ComponentModel.DataAnnotations;

namespace PustokApp.Models.Home
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
