﻿namespace GradProject_API.DTOs
{
    public class CartItemReadDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImg { get; set; }
        //public string ProductCondition { get; set; }
        //public string ProductStatus { get; set; }
    }
}
