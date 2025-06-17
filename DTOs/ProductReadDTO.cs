using System.ComponentModel.DataAnnotations;

namespace GradProject_API.DTOs
{
    public class ProductReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }        
        public string Size { get; set; }
        public string Color { get; set; }
        public string Img { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        //public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        //public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Rating { get; set; }
        public DateTime DatePosted { get; set; }


    }
}
