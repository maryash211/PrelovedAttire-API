namespace GradProject_API.DTOs
{
    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

    }
}
