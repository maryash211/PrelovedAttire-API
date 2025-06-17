namespace GradProject_API.DTOs
{
    public class CartReadDTO
    {
        //public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemReadDTO> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
