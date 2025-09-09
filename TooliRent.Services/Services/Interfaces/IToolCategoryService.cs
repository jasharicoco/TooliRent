using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IToolCategoryService
        : IGenericService<ToolCategory, ToolCategoryDto, CreateToolCategoryDto, UpdateToolCategoryDto>
    {
    }
}
