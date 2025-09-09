using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;

        public RentalsController(IRentalService service)
        {
            _service = service;
        }

        // GET: alla Rentals (Admin) eller filtrerat per användare/kund
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? userId = null,
            [FromQuery] int? customerId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                // Filtrera på GUID UserId (Identity)
                var bookingsByUser = await _service.GetByUserIdAsync(userId);
                return Ok(bookingsByUser);
            }

            if (customerId.HasValue)
            {
                // Filtrera på CustomerId (int)
                var bookingsByCustomer = await _service.GetByCustomerIdAsync(customerId.Value);
                return Ok(bookingsByCustomer);
            }

            // Alla bokningar
            var allBookings = await _service.GetAllAsync();
            return Ok(allBookings);
        }


        // GET: en Rental
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        // POST: skapa ny Rental (bokning)
        [HttpPost]
        [Authorize] // autentiserad användare
        public async Task<IActionResult> Create([FromBody] CreateRentalDto dto)
        {
            var created = await _service.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: uppdatera t.ex. slutdatum
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRentalDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // PATCH: uppdatera endast status
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRentalStatusDto dto)
        {
            try
            {
                var updated = await _service.UpdateStatusAsync(id, dto.Status);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: alla Rentals för den inloggade användaren
        [HttpGet("my-bookings")]
        [Authorize]
        public async Task<IActionResult> GetMyBookings()
        {
            // Hämta userId som string från JWT-tokenen
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var bookings = await _service.GetByUserIdAsync(userId);
            return Ok(bookings);
        }

        // DELETE: avboka en egen Rental
        [HttpDelete("my-bookings/{id:int}")]
        [Authorize]
        public async Task<IActionResult> CancelMyBooking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _service.CancelBookingByUserAsync(userId, id);
            return success ? NoContent() : Forbid();
        }


        // DELETE: ta bort Rental
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
