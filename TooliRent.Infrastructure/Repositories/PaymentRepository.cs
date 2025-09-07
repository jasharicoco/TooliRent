using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class PaymentRepository
        : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<Payment?> GetDetailedByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.Rental)
                .ThenInclude(r => r.Tool)
                .Include(p => p.Rental)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetAllDetailedAsync()
        {
            return await _context.Payments
                .Include(p => p.Rental)
                .ThenInclude(r => r.Tool)
                .Include(p => p.Rental)
                .ThenInclude(r => r.Customer)
                .ToListAsync();
        }

    }
}
