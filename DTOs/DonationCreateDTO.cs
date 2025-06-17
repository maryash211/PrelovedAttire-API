using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class DonationCreateDTO
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int CharityId { get; set; }

    }
}
