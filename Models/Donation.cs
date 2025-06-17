using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Description { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        
        public string City { get; set; }
        public string Address { get; set; }

        public int CharityId { get; set; }
        public virtual Charity Charity { get; set; }




    }
}
