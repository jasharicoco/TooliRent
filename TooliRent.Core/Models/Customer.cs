namespace TooliRent.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<Rental> Rentals { get; set; } = new();
    }
}
