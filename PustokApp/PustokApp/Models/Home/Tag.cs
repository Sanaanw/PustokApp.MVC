namespace PustokApp.Models.Home
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }
        public List<BookTag> BookTags { get; set; }

    }
}
