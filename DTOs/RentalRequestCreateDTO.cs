using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class RentalRequestCreateDTO
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Invalid National ID")]
        public string NationalId { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime RentalStartDate { get; set; }

        //public DateTime RequestDate { get; set; }
        //public string Status { get; set; } // Pending
        //public string PickupStatus { get; set; } //not picked - picked up
    }
}
