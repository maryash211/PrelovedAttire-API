using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(1,5)]
        public int Value { get; set; }
        public DateTime DatePosted { get; set; }

        //[ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
