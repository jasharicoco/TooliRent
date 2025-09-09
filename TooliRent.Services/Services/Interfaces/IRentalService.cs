using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IRentalService
        : IGenericService<Rental, RentalDto, CreateRentalDto, UpdateRentalDto>
    {
        Task<RentalDto> CreateBookingAsync(CreateRentalDto dto);
        Task<RentalDto?> UpdateStatusAsync(int id, RentalStatus status);
        Task<IEnumerable<RentalDto>> GetByUserIdAsync(string userId);
        Task<bool> CancelBookingByUserAsync(string userId, int bookingId);
        Task<IEnumerable<RentalDto>> GetByCustomerIdAsync(int customerId);

    }
}
