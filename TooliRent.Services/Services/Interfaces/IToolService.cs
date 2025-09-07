using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IToolService
        : IGenericService<Tool, ToolDto, CreateToolDto, UpdateToolDto>
    {
        Task<IEnumerable<RentalDto>> GetRentalsForTool(int toolId);
        Task<IEnumerable<ToolDto>> GetFilteredAsync(
            int? categoryId,
            string? condition,
            bool? availableOnly,
            DateTime? availableFrom,
            DateTime? availableTo);
    }
}
