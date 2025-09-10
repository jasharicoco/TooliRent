using TooliRent.Core.Models;

namespace TooliRent.Services.DTOs
{
    public record ToolDto(int Id, string Name, string Description, string? ImageUrl, string Condition, decimal Price, string ToolCategoryName);
    public record CreateToolDto(string Name, string Description, string? ImageUrl, ToolCondition Condition, decimal Price, int ToolCategoryId);
    public record UpdateToolDto(string Name, string Description, string? ImageUrl, ToolCondition Condition, decimal Price, int ToolCategoryId);
}