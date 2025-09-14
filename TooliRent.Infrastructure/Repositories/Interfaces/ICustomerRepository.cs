using TooliRent.Core.Models;

namespace TooliRent.Infrastructure.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByGuidAsync(string guid);
    }
}
