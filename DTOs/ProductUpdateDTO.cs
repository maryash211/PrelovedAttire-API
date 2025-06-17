using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class ProductUpdateDTO
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public string Address { get; set; }


        [MaxLength(20)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]

        public string? Size { get; set; }

        public string? Color { get; set; }

        [RegularExpression("^(New|Used)$")]
        public string? Condition { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
        //public int UserId { get; set; } //UserId of the seller
        
    }

}

