using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GradProject_API.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

    }
}
