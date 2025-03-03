using PustokApp.Models.Home;

namespace PustokApp.ViewModels
{
    public class BookDetailVm
    {
        public Book Book { get; set; }
        public List<Book> ReleatedBook { get; set; }
    }
}
