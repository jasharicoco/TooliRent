using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Repositories.Interfaces
{
    public interface IToolRepository : IGenericRepository<Tool>
    {
        Task<IEnumerable<Rental>> GetRentalsByToolIdAsync(int toolId);
    }
}
