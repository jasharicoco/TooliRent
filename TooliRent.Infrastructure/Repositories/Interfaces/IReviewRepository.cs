using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetByRentalIdAsync(int rentalId);
        Task<Rental?> GetRentalWithCustomerAsync(int rentalId, CancellationToken cancellationToken = default);
    }
}
