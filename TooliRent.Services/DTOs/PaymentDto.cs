using TooliRent.Core.Models;

namespace TooliRent.Services.DTOs
{
    public record PaymentDto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public record CreatePaymentDto(int RentalId, decimal Amount, PaymentMethod Method);

    public record UpdatePaymentDto(PaymentMethod? Method, PaymentStatus? Status);
}
