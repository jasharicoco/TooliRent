using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IRentalService
        : IGenericService<Rental, RentalDto, CreateRentalDto, UpdateRentalDto>
    {
        // Skapa bokningar
        Task<RentalDto> CreateBookingByUserAsync(CreateRentalByUserDto dto);
        Task<RentalDto> CreateBookingByCustomerAsync(CreateRentalByCustomerDto dto);

        // Uppdatera status på en bokning
        Task<RentalDto?> UpdateStatusAsync(int id, RentalStatus status);

        // Hämta bokningar
        Task<IEnumerable<RentalDto>> GetByUserIdAsync(string userId);
        Task<IEnumerable<RentalDto>> GetByCustomerIdAsync(int customerId);

        // Avboka bokning
        Task<bool> CancelBookingByUserAsync(string userId, int bookingId);

        // Statistik
        Task<RentalStatisticsDto> GetStatisticsAsync();
    }
}
