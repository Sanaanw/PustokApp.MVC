using PustokApp.Models.Home;

namespace PustokApp.ViewModels
{
    public class BookDetailVm
    {
        public Book Book { get; set; }
        public List<Book> ReleatedBook { get; set; }
        public int TotalComments { get; set; }
        public bool HasComment { get; set; }
        public decimal AvgRate { get; set; }
        public BookComment BookComment { get; set; }
    }
}
