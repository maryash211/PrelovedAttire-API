using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class ContactUsDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get;set; }

        [Required]
        public string Message { get; set; }
    }
}
