namespace GradProject_API.DTOs
{
    public class OrderReadDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } //Remove if not needed
        public string UserName { get; set; } //Remove if not needed
        public string UserEmail { get; set; } //Remove if not needed
        public string UserPhoneNumber { get; set; } //Remove if not needed
        public string UserAddress { get; set; } //Remove if not needed
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemReadDTO> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
