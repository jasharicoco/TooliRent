using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IToolService
        : IGenericService<Tool, ToolDto, CreateToolDto, UpdateToolDto>
    {
        Task<IEnumerable<RentalDto>> GetRentalsForTool(int toolId);
    }
}
