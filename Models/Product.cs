using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class Product
    {
        public Product()
        {
            Ratings = new HashSet<Rating>();
        }
        public string UserName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }


        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string ImageUrl { get; set; }


        [Range(1, 5)]
        public decimal Rating { get; set; }
        public string Status { get; set; }
        public DateTime DatePosted { get; set; }


        //Rent-only fields
        public string Type { get; set; } // sell/rent
        //public int Duration { get; set; }
        //public DateTime RentalStartDate { get; set; }

        //public decimal SecurityDeposit { get; set; }
        //public string ReturnPolicy { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } 
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<SwapRequest> SwapRequests { get; set; }
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }

    }
}
