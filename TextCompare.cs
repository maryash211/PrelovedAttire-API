//using GradProject_API.DTOs;
//using GradProject_API.Models;
//using Microsoft.AspNetCore.Mvc;

//[Route("api/[controller]")]
//[ApiController]
//public class RatingsController : ControllerBase
//{
//    private readonly GradDbContext _context;

//    public RatingsController(GradDbContext context)
//    {
//        _context = context;
//    }

//    [HttpPost]
//    public async Task<ActionResult<RatingReadDTO>> CreateRating(RatingCreateDTO ratingDto)
//    {
//        // Get product with owner info
//        var product = await _context.Products
//            .Include(p => p.User)
//            .FirstOrDefaultAsync(p => p.Id == ratingDto.ProductId);

//        if (product == null) return NotFound("Product not found");

//        // Prevent self-rating (remove when we have auth)
//        if (/* currentUserId */ 1 == product.UserId) // Replace with User.FindFirstValue when auth is implemented
//        {
//            return BadRequest("You cannot rate your own product");
//        }

//        var rating = new Rating
//        {
//            Value = ratingDto.Value,
//            Comment = ratingDto.Comment,
//            ProductId = ratingDto.ProductId,
//            UserId = 1, // Replace with actual user ID from auth
//            DateCreated = DateTime.Now
//        };

//        _context.Ratings.Add(rating);
//        await _context.SaveChangesAsync();

//        // Update product average rating
//        await UpdateProductRating(ratingDto.ProductId);

//        return CreatedAtAction("GetRating", new { id = rating.Id }, new RatingReadDTO
//        {
//            Id = rating.Id,
//            Value = rating.Value,
//            Comment = rating.Comment,
//            DateCreated = rating.DateCreated,
//            ProductId = rating.ProductId,
//            UserName = "Current User" // Replace with actual user name
//        });
//    }


//    private async Task UpdateProductRating(int productId)
//    {
//        var product = await _context.Products.FindAsync(productId);
//        if (product != null)
//        {
//            product.Rating = await _context.Ratings
//                .Where(r => r.ProductId == productId)
//                .AverageAsync(r => r.Value);

//            await _context.SaveChangesAsync();
//        }

//    }






//    [HttpGet("product/{productId}")]
//    public async Task<ActionResult<IEnumerable<RatingReadDTO>>> GetProductRatings(int productId)
//    {
//        return await _context.Ratings
//            .Where(r => r.ProductId == productId)
//            .Include(r => r.User)
//            .Select(r => new RatingReadDTO
//            {
//                Id = r.Id,
//                Value = r.Value,
//                Comment = r.Comment,
//                DateCreated = r.DateCreated,
//                ProductId = r.ProductId,
//                UserName = r.User.Name
//            })
//            .ToListAsync();
//    }
//    }
//}