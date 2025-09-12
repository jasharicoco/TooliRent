using TooliRent.Core.Models;

namespace TooliRent.Services.DTOs
{
    public record PaymentDto
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }

    public record CreatePaymentDto(int RentalId, decimal Amount, string Method);

    public record UpdatePaymentDto(string? Method, string? Status);

    public record UpdatePaymentStatusDto(PaymentStatus Status);
    public record UpdatePaymentMethodDto(PaymentMethod Method);
}