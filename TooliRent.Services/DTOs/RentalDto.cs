using TooliRent.Core.Models;

namespace TooliRent.Services.DTOs
{
    public record RentalDto
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public string CustomerName { get; init; }
        public int ToolId { get; init; }
        public string ToolName { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public decimal TotalPrice { get; init; }
        public string Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime ModifiedAt { get; init; }
    }

    public record CreateRentalDto(string CustomerId, int ToolId, DateTime StartDate, DateTime EndDate);

    public record CreateRentalByUserDto(string UserId, int ToolId, DateTime StartDate, DateTime EndDate);

    public record CreateRentalByCustomerDto(int CustomerId, int ToolId, DateTime StartDate, DateTime EndDate);

    public record UpdateRentalDto(DateTime? EndDate);

    public record UpdateRentalStatusDto(RentalStatus Status);
}
