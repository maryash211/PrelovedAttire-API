using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }     

        public int Quantity { get; set; }//How many of that product are in the cart

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("CartId")]
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }




    }
}
