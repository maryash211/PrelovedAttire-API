using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } //Pending/Completed/Cancelled

        [ForeignKey("User")]
        public string UserId { get; set; }  // Changed from int to string
        public virtual ApplicationUser User { get; set; }  // Changed from User to ApplicationUser

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
