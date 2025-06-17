using GradProject_API.DTOs;
using GradProject_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly GradDbContext _context;

        public DonationsController(GradDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Donation>> CreateDonation([FromBody] DonationCreateDTO donationDto)
        {
            var donation = new Donation
            {
                Name = donationDto.Name,
                Email = donationDto.Email,
                PhoneNumber = donationDto.PhoneNumber,
                City = donationDto.City,
                Address = donationDto.Address,
                Description = donationDto.Description,
                CharityId = donationDto.CharityId
            };
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonation", new { id = donation.Id }, donation);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> GetAllDonations()
        {
            var donations = await _context.Donations.ToListAsync();
            if (donations == null || !donations.Any())
            {
                return NotFound("No donations found");
            }
            return await _context.Donations.Include(d=> d.Charity).ToListAsync();
        }

    }
}
