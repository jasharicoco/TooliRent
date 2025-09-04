namespace TooliRent.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set; }
        public int Rating { get; set; } // 1–5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
