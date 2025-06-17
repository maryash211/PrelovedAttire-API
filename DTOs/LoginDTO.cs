using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
