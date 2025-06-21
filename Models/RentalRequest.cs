using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class RentalRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNUmber { get; set; }
        public string NationalId { get; set; }
        public int Duration { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RequestDate { get; set; }

        public string Status { get; set; } // Pending
        public string PickupStatus { get; set; } //not picked - picked up

        //[ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        //[ForeignKey("RenterId")]
        public string RenterId { get; set; } //the person who pays money to rent a product (not the owner)
        public virtual ApplicationUser Renter { get; set; }
    }
}
