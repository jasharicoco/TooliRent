using Microsoft.EntityFrameworkCore;

namespace TooliRent.Core.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ToolId { get; set; }
        public Tool Tool { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; } // enum
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public Review Review { get; set; } // 1-1
    }
}
