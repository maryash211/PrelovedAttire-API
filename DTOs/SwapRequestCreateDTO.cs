using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class SwapRequestCreateDTO
    {
        [Required]
        public string RequesterName { get; set; }

        //public string RequesterId { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Invalid National ID")]
        public string NationalId { get; set; }

        [Required]
        public string OfferedProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public IFormFile ProductImageUrl { get; set; }

        public int RequestedProductId { get; set; }
        public int OfferedProductId { get; set; }

    }
}
