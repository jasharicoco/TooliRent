using Microsoft.EntityFrameworkCore;

namespace TooliRent.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public Rental Rental { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; } // enum
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; } // enum
    }
}
