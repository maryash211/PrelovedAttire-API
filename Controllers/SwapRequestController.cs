using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwapRequestController : ControllerBase
    {
        private readonly GradDbContext _context;
        private readonly IWebHostEnvironment _env;
        private const string SwapImagesFolder = "Swap-images";
        public SwapRequestController(GradDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateSwapRequest([FromForm] SwapRequestCreateDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var requestedProduct = await _context.Products.FindAsync(dto.RequestedProductId);
            var offeredProduct = await _context.Products.FindAsync(dto.OfferedProductId);


            if (requestedProduct == null || offeredProduct == null)
            {
                return BadRequest("Requested/Offered product or both not found.");
            }

            if (requestedProduct.UserId == userId)
            {
                return Forbid("You cannot request a swap for your own product.");
            }

            if (requestedProduct.Status != "Available")
            {
                return BadRequest("The product you want is not available for swap.");
            }

            var existingRequest = await _context.SwapRequests.FirstOrDefaultAsync(r => r.RequestedProductId == dto.RequestedProductId &&
                              r.OfferedProductId == dto.OfferedProductId &&
                              r.RequesterId == userId &&
                              r.Status == "Pending");
            if (existingRequest != null)
            {
                return BadRequest("You already made this swap request.");
            }


            if (dto.ProductImageUrl == null || dto.ProductImageUrl.Length == 0)
            {
                return BadRequest("No file uploaded,Image is required");
            }
            var extension = Path.GetExtension(dto.ProductImageUrl.FileName).ToLower();
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!validExtensions.Contains(extension))
            {
                return BadRequest($"Invalid Image Format. Allowed formats: {string.Join(",", validExtensions)}");
            }
            var folderPath = Path.Combine(_env.WebRootPath, SwapImagesFolder);
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ProductImageUrl.CopyToAsync(stream);
            }
            var savedImagePath = $"/{SwapImagesFolder}/{fileName}";
            var swap = new SwapRequest
            {
                RequesterName = dto.RequesterName,
                RequesterId = userId,
                PhoneNumber = dto.PhoneNumber,
                NationalId = dto.NationalId,

                OfferedProductName = dto.OfferedProductName,
                Price = dto.Price,
                Size = dto.Size.ToString(),
                Color = dto.Color,
                ProductImageUrl = savedImagePath,

                RequestedProductId = dto.RequestedProductId,
                OfferedProductId = dto.OfferedProductId,

                Status = "Pending",
                RequestDate = DateTime.UtcNow
            };
            _context.SwapRequests.Add(swap);
            await _context.SaveChangesAsync();

            return Ok("Swap request submitted");




        }
    }
}
