namespace GradProject_API.DTOs
{
    public class OrderCreateDTO
    {
        public string UserId { get; set; }
        public List<OrderItemCreateDTO> OrderItems { get; set; }

    }
}
