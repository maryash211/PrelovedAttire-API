using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly GradDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingsController(GradDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<RatingReadDTO>>> GetProductRatings (int productId)
        {
            
            return await _context.Ratings
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .Select(r => new RatingReadDTO
                {
                    Id = r.Id,
                    Value = r.Value,
                    DatePosted = r.DatePosted,
                    ProductId = r.ProductId,
                    UserName = r.User.FullName 
                })
                .ToListAsync();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RatingReadDTO>> AddRating(RatingCreateDTO ratingDto)
        {
            var product = await _context.Products
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == ratingDto.ProductId);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //returns a string
            var user = await _userManager.FindByIdAsync(userId);

            if (product == null) return NotFound("Product not found");

            if (userId == product.UserId)
            {
                return BadRequest("You cannot rate your own product");
            }
            var rating = new Rating
            {
                Value = ratingDto.Value,
                ProductId = ratingDto.ProductId,
                UserId = userId, 
                DatePosted = DateTime.Now
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            // Update product average rating
            await UpdateProductRating(ratingDto.ProductId);

            return CreatedAtAction(nameof(GetProductRatings), new { productid = rating.ProductId }, new RatingReadDTO
            {
                Id = rating.Id,
                Value = rating.Value,
                DatePosted = rating.DatePosted,
                ProductId = rating.ProductId,
                UserName = user.FullName
            });
        }

        private async Task UpdateProductRating(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                product.Rating = (decimal)await _context.Ratings
                    .Where(r => r.ProductId == productId)
                    .AverageAsync(r => r.Value);

                await _context.SaveChangesAsync(); 
            }



        }
    }
}