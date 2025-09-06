using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IRentalService
        : IGenericService<Rental, RentalDto, CreateRentalDto, UpdateRentalDto>
    {
        Task<RentalDto> CreateBookingAsync(CreateRentalDto dto);
    }
}
