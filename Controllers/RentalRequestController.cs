using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalRequestController : ControllerBase
    {
        private readonly GradDbContext _context;

        public RentalRequestController(GradDbContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateRentalRequest([FromBody] RentalRequestCreateDTO dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null || product.Type.ToLower() != "rent")
            {
                return BadRequest("Product not available for rent");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingRequest = await _context.RentalRequests.FirstOrDefaultAsync(r => r.ProductId == dto.ProductId && r.RenterId == userId && r.Status == "Pending");
            if (existingRequest != null)
            {
                return BadRequest("You have already requested to rent this product");
            }
            if (userId == product.UserId) // Fixed the condition to check if the user is trying to rent their own product  
            {
                return Forbid("You can't rent your own product");
            }
            var rentalRequest = new RentalRequest
            {
                Name = dto.Name,
                PhoneNUmber = dto.PhoneNumber,
                NationalId = dto.NationalId,
                Duration = dto.Duration,
                RentalStartDate = dto.RentalStartDate,
                ProductId = dto.ProductId,
                RequestDate = DateTime.UtcNow,
                Status = "Pending",
                PickupStatus = "Not Picked",
                RenterId = userId
            };
            _context.RentalRequests.Add(rentalRequest);
            await _context.SaveChangesAsync();
            return Ok("Rental request submitted");
        }
    }
}
