using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
