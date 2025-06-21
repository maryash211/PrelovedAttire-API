using System.ComponentModel.DataAnnotations.Schema;

namespace GradProject_API.Models
{
    public class OrderItem // Will have Purchase history (Zay Fatoora)
    {
        public int Id { get; set; }

        //public string Name { get; set; } DTO for that?

        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        //[ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        //[ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }





    }
}
