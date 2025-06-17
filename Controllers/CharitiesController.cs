using GradProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharitiesController : ControllerBase
    {
        private readonly GradDbContext _context;

        public CharitiesController(GradDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<Charity>> GetAllCharities() //just in case we need that
        {
            var charities = await _context.Charities.ToListAsync();
            if (charities == null || !charities.Any())
            {
                return NotFound("No charities found");
            }
            return Ok(charities);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Charity>> AddCharity(Charity charity)
        {
            _context.Charities.Add(charity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllCharities), new { id = charity.Id }, charity);
        }
    }
}
