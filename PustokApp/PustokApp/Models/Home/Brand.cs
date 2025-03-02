namespace PustokApp.Models.Home
{
    public class Brand:BaseEntity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
