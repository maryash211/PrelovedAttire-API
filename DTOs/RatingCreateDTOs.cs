using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class RatingCreateDTO
    {
        [Required]
        [Range(1, 5)]
        public int Value { get; set; }

        [Required]
        public int ProductId { get; set; }
    }


    public class RatingReadDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DatePosted { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
    }
}
