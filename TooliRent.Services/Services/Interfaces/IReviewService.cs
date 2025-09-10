using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IReviewService : IGenericService<Review, ReviewDto, CreateReviewDto, UpdateReviewDto>
    {
        Task<IEnumerable<ReviewDto>> GetByRentalIdAsync(int rentalId);
    }
}
