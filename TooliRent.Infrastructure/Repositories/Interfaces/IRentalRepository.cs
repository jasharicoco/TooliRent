using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Repositories.Interfaces
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<bool> ToolIsAvailableAsync(int toolId, DateTime startDate, DateTime endDate);
        Task<Rental?> GetDetailedByIdAsync(int id); // inkluderar Tool & Customer
        Task<IEnumerable<Rental>> GetAllDetailedAsync(); // inkluderar Tool & Customer
        Task<IEnumerable<Rental>> GetByUserIdAsync(string userId);
        Task<bool> DeleteAsync(Rental rental);
        Task<IEnumerable<Rental>> GetByCustomerIdAsync(int customerId);

    }
}
