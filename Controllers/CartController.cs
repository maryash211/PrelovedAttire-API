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
    public class CartController : ControllerBase
    {
        private readonly GradDbContext _context;

        public CartController(GradDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CartReadDTO>> GetCart(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartItems = cart.CartItems.Select(ci => new CartItemReadDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                ProductImg = ci.Product.ImageUrl,
            }).ToList();

            return new CartReadDTO
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(ci => ci.ProductPrice * ci.Quantity)
            };
        }

        [Authorize]
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddToCart(string userId, CartItemCreateDTO cartItemDto)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId); //cart of user

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(cartItemDto.ProductId); //find from products in db the product that has the id of the product chosen by the user
                                                                                    // (P.S.the productfrom db the user chose to add to the cart)
            if (product == null)
            {
                return NotFound("Product not found");
            }
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == cartItemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity; //but thats only when the product sold has quantity
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity,
                    Name = product.Name,
                    Price = product.Price,
                };
                _context.CartItems.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product added to cart successfully" });
        }

        [Authorize]
        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> DeleteCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            _context.CartItems.Remove(cartItem); //Change status then??
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product removed from cart successfully" });

        }
    }
}
