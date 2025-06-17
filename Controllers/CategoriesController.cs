using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GradDbContext _context;

        public CategoriesController(GradDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found");
            }
            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task <ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            var allCategories = await _context.Categories.ToListAsync();
            return Ok(allCategories);
        }
    }
}
