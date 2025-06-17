using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class SwapRequest
    {
        public int Id { get; set; }
        public string RequesterName { get; set; }
        public string OfferedProductName { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; } 
        public string ProductImageUrl { get; set; }


        public string Status { get; set; } //pending
        public DateTime RequestDate { get; set; }

        [ForeignKey("RequestedProductId")]
        public int RequestedProductId { get; set; }
        public virtual Product RequestedProduct { get; set; }

        public int OfferedProductId { get; set; }
        public virtual Product OfferedProduct { get; set; }


        [ForeignKey("RequesterId")]
        public string RequesterId { get; set; }
        public virtual ApplicationUser Requester { get; set; }
    }
}
