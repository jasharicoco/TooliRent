namespace TooliRent.Services.DTOs
{
    public record ToolCategoryDto(int Id, string Name, string Description);

    public record CreateToolCategoryDto(string Name, string Description);

    public record UpdateToolCategoryDto(string? Name, string? Description);
}
