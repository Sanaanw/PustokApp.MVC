using System.ComponentModel.DataAnnotations;

namespace PustokApp.Models.Home
{
    public class BookComment : BaseEntity
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Text { get; set; }
        public int Rate { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Pending;
    }
    public enum CommentStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
