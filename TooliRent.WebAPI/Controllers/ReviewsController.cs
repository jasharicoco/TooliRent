using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _service.GetAllAsync();
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _service.GetByIdAsync(id);
            if (review is null) return NotFound();
            return Ok(review);
        }

        [HttpGet("rental/{rentalId:int}")]
        public async Task<IActionResult> GetByRental(int rentalId)
        {
            var reviews = await _service.GetByRentalIdAsync(rentalId);
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Validering av hyrning sker i ReviewValidator
            var review = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        //// Uppdatering av recensioner är inte tillåtet i nuläget
        //[HttpPut("{id:int}")]
        //[Authorize]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
        //{
        //    var updated = await _service.UpdateAsync(id, dto);
        //    if (updated is null) return NotFound();
        //    return Ok(updated);
        //}

        [HttpDelete("{id:int}")]
        [Authorize] // Admin så småningom
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
