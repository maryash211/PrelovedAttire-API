namespace GradProject_API.DTOs
{
    public class OrderItemReadDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

    }
}
