using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class CustomerRepository
        : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public async Task<Customer> GetByGuidAsync(string guid)
        {
            return await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == guid);
        }
    }
}
