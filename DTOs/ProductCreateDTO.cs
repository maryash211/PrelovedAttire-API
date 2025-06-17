using System.ComponentModel.DataAnnotations;
//using System.Drawing;

namespace GradProject_API.DTOs
{
    public class ProductCreateDTO
    {
        //user info
        [Required]
        public string UserName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Invalid National ID")]
        public string NationalId { get; set; }
        public string? Address { get; set; }



        [MaxLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]

        public string Size { get; set; }

        public string Color { get; set; }

        public int CategoryId { get; set; }
        //public int UserId { get; set; } //UserId of the seller

        [Required]
        public IFormFile Image { get; set; }

        //extras for rental
        public string Type { get; set; }
        //public int Duration { get; set; }
        //public DateTime RentalStartDate { get; set; }





    }
}
