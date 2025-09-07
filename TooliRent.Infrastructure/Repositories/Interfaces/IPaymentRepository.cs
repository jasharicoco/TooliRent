using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Repositories.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetDetailedByIdAsync(int id);
        Task<IEnumerable<Payment>> GetAllDetailedAsync();
    }
}
