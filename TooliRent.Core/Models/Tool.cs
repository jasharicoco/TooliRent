namespace TooliRent.Core.Models
{
    public class Tool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public ToolCondition Condition { get; set; } // enum
        public decimal Price { get; set; }
        public int ToolCategoryId { get; set; }
        public ToolCategory ToolCategory { get; set; }
        public List<Rental> Rentals { get; set; }
    }
}
