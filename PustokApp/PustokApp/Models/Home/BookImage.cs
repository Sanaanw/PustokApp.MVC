namespace PustokApp.Models.Home
{
    public class BookImage:BaseEntity
    {
        public string Name { get; set; }
        public int BookId { get; set; }
        public bool? Status { get; set; }
        public Book Book { get; set; }
    }
}
