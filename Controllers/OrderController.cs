using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradProject_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GradDbContext _context;

        public OrderController(GradDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderReadDTO>> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            var  user = await _context.Users.FindAsync(orderDto.UserId);
            if ( user == null) return BadRequest("User not found");
     

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalPrice = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            decimal totalPrice = 0;
            var items = new List<OrderItem>();
            var itemDtos = new List<OrderItemReadDTO>();

            foreach (var itemDto in orderDto.OrderItems) //itemDto now is each item in OrderItems of Order created by user
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null) continue; //write a message to the user that product not found

                //if (product.Quantity < itemDto.Quantity)
                //{
                //    return BadRequest($"Not enough stock for product {product.Name}");
                //}

                //product.Quantity -= itemDto.Quantity;

                //if (product.Quantity == 0)
                //{
                //    product.Status = "Sold Out";
                //}

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    PriceAtPurchase = product.Price,
                    OrderId = order.Id
                };

                items.Add(orderItem);
                totalPrice += product.Price * itemDto.Quantity;

                itemDtos.Add(new OrderItemReadDTO
                {
                    ProductId = orderItem.ProductId,
                    ProductName = product.Name,
                    PriceAtPurchase = orderItem.PriceAtPurchase,
                    Quantity = orderItem.Quantity

                });
            }

            order.TotalPrice = totalPrice;
            await _context.OrderItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();

            return Ok(new OrderReadDTO
            {
                Id = order.Id,
                //UserId = order.UserId,
                UserName = user.FullName,  //Remove if not necessary
                UserEmail = user.Email, //Remove if not necessary
                UserPhoneNumber = user.PhoneNumber, //Remove if not necessary
                //UserAddress = user.Address, //Remove if not necessary
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderItems = itemDtos,
            });

        }
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetUserOrders(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new OrderReadDTO
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    UserName = o.User.FullName, //Remove if not necessary
                    UserEmail = o.User.Email, //Remove if not necessary
                    UserPhoneNumber = o.User.PhoneNumber, //Remove if not necessary
                    //UserAddress = o.User.Address, //Remove if not necessary
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemReadDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        Quantity = oi.Quantity
                    }).ToList()


                })
                .ToListAsync();

            return Ok(orders);
        }
           



    }
}
