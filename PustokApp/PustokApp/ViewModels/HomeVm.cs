using PustokApp.Models.Home;

namespace PustokApp.ViewModels
{
    public class HomeVm
    {
        public List<Slider> Sliders { get; set; }
        public List<Author> Authors { get; set; }
        public List<BookImage> BookImages { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Book> Books { get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> DiscountBooks { get; set; }
        public List<Feature> Features { get; set; }

    }
}
