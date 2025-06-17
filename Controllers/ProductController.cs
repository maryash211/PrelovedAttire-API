using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace GradProject_API.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GradDbContext _context;
        private readonly IWebHostEnvironment _env;
        private const string ProductImageFolder = "product-images";


        public ProductController(GradDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAllProducts()
        {
            if (_context.Products == null)
            {
                return NotFound("No products found");
            }
            else
            {
                return await _context.Products
                    .Include(p => p.Category) //          REMOVE???????
                    .Include(p => p.User)    //          REMOVE???????
                    .Select(p => new ProductReadDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Size = p.Size,
                        Color = p.Color,
                        Img = p.ImageUrl,
                        Status = p.Status,
                        CategoryName = p.Category.Name,
                        UserName = p.User.FullName,
                        Rating = p.Rating,
                        DatePosted = p.DatePosted

                    })
                    .ToListAsync();
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not Found");
            }
            return new ProductReadDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Size = product.Size,
                Color = product.Color,
                Img = product.ImageUrl,
                Status = product.Status,
                CategoryName = product.Category.Name,
                UserName = product.User.FullName,
                Rating = product.Rating,
                DatePosted = product.DatePosted
            };
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetUserProducts(string userId)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.UserId == userId)
                    .Select(p => new ProductReadDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Size = p.Size,
                        Color = p.Color,
                        Img = p.ImageUrl,
                        Status = p.Status,
                        CategoryName = p.Category.Name,
                        UserName = p.User.FullName,
                        Rating = p.Rating,
                        DatePosted = p.DatePosted
                    })
                    .ToListAsync();

                if (products == null || !products.Any()) 
                {
                    return NotFound("No products found for this user");
                }
                else
                {
                    return Ok(products);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving products: {ex.Message}");
            }
        }



        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<ActionResult<ProductReadDTO>> AddProduct([FromForm]ProductCreateDTO productdto)
        {
            if (ModelState.IsValid)
            {
                //for Image upload functionality
                if (productdto.Image == null || productdto.Image.Length == 0)
                {
                    return BadRequest("No file uploaded,Image is required");
                }
                var extension = Path.GetExtension(productdto.Image.FileName).ToLower();
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!validExtensions.Contains(extension))
                {
                    return BadRequest($"Invalid Image Format. Allowed formats: {string.Join(",",validExtensions)}");
                }
                
                //directory folder (product-Images file in wwroot)
                var folderPath = Path.Combine(_env.WebRootPath, ProductImageFolder);
                Directory.CreateDirectory(folderPath);

                //file name
                var fileName = $"{Guid.NewGuid()}{extension}"; // same as var fileName = Guid.NewGuid() + extension;
                var filePath = Path.Combine(folderPath, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productdto.Image.CopyToAsync(stream); //Copying the file to the specified path.
                }
                var savedImagePath = $"/{ProductImageFolder}/{fileName}";


                var categoryExists = await _context.Categories.AnyAsync(c => c.Id == productdto.CategoryId);
                if (!categoryExists)
                {
                    return BadRequest("Invalid category");
                }

                var product = new Product
                {
                    UserName = productdto.UserName,
                    PhoneNumber = productdto.PhoneNumber,
                    NationalId = productdto.NationalId,
                    Address = productdto.Address,

                    Name = productdto.Name,
                    Description = productdto.Description,
                    Price = productdto.Price,
                    Size = productdto.Size.ToString(),
                    Color = productdto.Color,
                    ImageUrl = savedImagePath, //where??  Set the image URL to the file name
                    CategoryId = productdto.CategoryId,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Rating = 0,
                    Status = "Available",
                    DatePosted = DateTime.Now
                };
                if (productdto.Type.ToLower() == "rent")
                {
                    product.Status = "Available for rent";
                    //product.Duration = productdto.Duration;
                    //product.RentalStartDate = productdto.RentalStartDate;
                }
                
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var productReadDto = new ProductReadDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Size = product.Size,
                    Color = product.Color,
                    Img = product.ImageUrl,
                    Status = product.Status,
                    CategoryName = product.Category.Name,//didnt add categoryid
                    UserName = product.User.FullName,        //didnt add userid
                    Rating = product.Rating,
                    DatePosted = product.DatePosted
                };

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productReadDto);
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<ProductReadDTO>> UpdateProduct(int id, [FromForm] ProductUpdateDTO productDto)
        {
            try
            {

                var product = await _context.Products.FindAsync(id);
                if (product == null) return NotFound();
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");

                if (product.UserId != currentUserId && !isAdmin)
                {
                    return Forbid("You are not authorized to update this product");
                }
                if (productDto.CategoryId.HasValue &&
                productDto.CategoryId != product.CategoryId)
                {
                    var categoryExists = await _context.Categories
                        .AnyAsync(c => c.Id == productDto.CategoryId);
                    if (!categoryExists) return BadRequest("Invalid category");
                }
                string imageUrl = product.ImageUrl;

                if (productDto.Image != null) //did the user add new image in update
                {
                    //if yes, then we dont need the old photo so we delete it

                    if (!string.IsNullOrEmpty(imageUrl)) //if that old photo path somehow not empty yet (exists)
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/')); //This is like  C:\\MyApp\\wwwroot +  /product-images/12345abcde.png (but remove the first slash(/product-images))

                        //	System.IO is a .NET namespace for file and directory operations.
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    //After deleting, Save new image


                    var extension = Path.GetExtension(productDto.Image.FileName).ToLower();
                    var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!validExtensions.Contains(extension))
                    {
                        return BadRequest($"Invalid Image Format. Allowed formats: {string.Join(",", validExtensions)}");
                    }
                    var folderPath = Path.Combine(_env.WebRootPath, ProductImageFolder);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(folderPath, fileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productDto.Image.CopyToAsync(stream);
                    }
                    imageUrl = $"/{ProductImageFolder}/{fileName}";

                }
                product.UserName = productDto.UserName ?? product.UserName;
                product.PhoneNumber = productDto.PhoneNumber ?? product.PhoneNumber;
                product.NationalId = productDto.NationalId ?? product.NationalId;
                product.Address = productDto.Address ?? product.Address;

                product.Name = productDto.Name ?? product.Name;
                product.Description = productDto.Description ?? product.Description;
                product.Price = productDto.Price ?? product.Price;
                product.Size = productDto.Size.ToString() ?? product.Size.ToString();
                product.Color = productDto.Color ?? product.Color;
                product.CategoryId = productDto.CategoryId ?? product.CategoryId;
                product.DatePosted = DateTime.Now;
                product.ImageUrl = imageUrl;

                await _context.SaveChangesAsync();

                var productReadDto = new ProductReadDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Size = product.Size,
                    Color = product.Color,
                    Img = product.ImageUrl,
                    Status = product.Status,
                    CategoryName = product.Category.Name,//didnt add categoryid
                    UserName = product.User.FullName,        //didnt add userid
                    Rating = product.Rating,
                    DatePosted = product.DatePosted
                };

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productReadDto);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Error updating product: {ex.Message}");
            }
        }
        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.CartItems)
                    .FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null) return NotFound("Product not found");

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Admin");
                if (product.UserId != currentUserId && !isAdmin)
                {
                    return Forbid("You are not authorized to update this product");
                }
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var imagePath = Path.Combine(_env.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _context.CartItems.RemoveRange(product.CartItems); //RemoveRange removes elements from a LIST
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }


        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<ProductReadDTO>>> SearchProducts([FromQuery] string searchTerm, [FromQuery] int? categoryId = null, [FromQuery] string? Type = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Include(p => p.Type)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.Trim().ToLower(); 
                query = query.Where(p => p.Name.ToLower().Contains(normalizedSearchTerm) || p.Description.ToLower().Contains(normalizedSearchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(Type))
            {
                query = query.Where(p => p.Type.ToLower() == Type.ToLower());
            }

            return await query.Select(p => new ProductReadDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Size = p.Size,
                Color = p.Color,
                Img = p.ImageUrl,
                Status = p.Status,
                CategoryName = p.Category.Name,
                UserName = p.User.FullName,
                Rating = p.Rating,
                DatePosted = p.DatePosted
            })
                .ToListAsync();
        }

    }
}