using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [Phone]
        public string PhoneNumber { get; set; }


        [Required]
        [StringLength(50)]
        public string Password { get; set; }



        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
