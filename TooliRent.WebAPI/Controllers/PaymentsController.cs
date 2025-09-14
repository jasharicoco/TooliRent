using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/payments
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return payment is null ? NotFound() : Ok(payment);
        }

        // PATCH: api/payments/5/status
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePaymentStatusDto dto)
        {
            var updated = await _paymentService.UpdateStatusAsync(id, dto.Status);
            return updated is null ? NotFound() : Ok(updated);
        }

        // PATCH: api/payments/5/method
        [HttpPatch("{id}/method")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> UpdateMethod(int id, [FromBody] UpdatePaymentMethodDto dto)
        {
            var updated = await _paymentService.UpdateMethodAsync(id, dto.Method);
            return updated is null ? NotFound() : Ok(updated);
        }

    }
}
