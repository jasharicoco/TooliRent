namespace TooliRent.Core.Models
{
    public class ToolCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tool> Tools { get; set; } = new();
    }
}
