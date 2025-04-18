using PustokApp.Areas.Manage.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokApp.Models.Home
{
    public class Book:BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool InStock { get; set; }
        public string ProductCode { get; set; }
        public string Availability { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<BookImage> BookImages { get; set; } = new();
        public List<BookTag> BookTags { get; set; }
        [NotMapped]
        [AllowedType("image/jpeg", "image/png")]
        [AllowedLength(2 * 1024 * 1024)]
        public List<IFormFile> Files { get; set; }
        [NotMapped]
        [AllowedType("image/jpeg", "image/png")]
        [AllowedLength(2 * 1024 * 1024)]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        [AllowedType("image/jpeg", "image/png")]
        [AllowedLength(2 * 1024 * 1024)]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        public List<int> ImgIds { get; set; }
        public List<BookComment> BookComments { get; set; }
        public Book()
        {
            BookComments = new();
        }
    }
}
