using System.ComponentModel.DataAnnotations;

namespace PustokApp.Models.Home
{
    public class Order:BaseEntity
    {
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Address { get; set; }
        public int TotalPrice { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public List<OrderItem> OrderItems { get; set; }
    }
}
public enum OrderStatus
{
    Pending,
    Accepted,
    Rejected,
    Cancelled
}
