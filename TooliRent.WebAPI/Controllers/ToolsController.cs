using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly IToolService _service;

        public ToolController(IToolService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] string? condition,
            [FromQuery] bool? availableOnly,
            [FromQuery] DateTime? availableFrom,
            [FromQuery] DateTime? availableTo)
        {
            var tools = await _service.GetFilteredAsync(categoryId, condition, availableOnly, availableFrom, availableTo);
            return Ok(tools);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tool = await _service.GetByIdAsync(id);
            return tool is null ? NotFound() : Ok(tool);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateToolDto dto)
        {
            var tool = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = tool.Id }, tool);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateToolDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{id}/rentals")]
        public async Task<IActionResult> GetRentals(int id)
            => Ok(await _service.GetRentalsForTool(id));
    }
}
