using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Rental?> GetDetailedByIdAsync(int id)
        {
            return await _context.Rentals
                .Include(r => r.Tool)                          // Verktyget
                .Include(r => r.Customer)                      // Kunden
                    .ThenInclude(c => c.User)                  // Identity / AppUser
                .Include(r => r.Payments)                      // Betalningar
                .Include(r => r.Review)                        // Recension
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ToolIsAvailableAsync(int toolId, DateTime startDate, DateTime endDate)
        {
            return !await _context.Rentals
                .Where(r => r.ToolId == toolId && r.Status != RentalStatus.Cancelled)
                .AnyAsync(r => r.StartDate < endDate && r.EndDate > startDate);
        }

        public override async Task<Rental?> GetByIdAsync(int id)
            => await GetDetailedByIdAsync(id);

        public async Task<IEnumerable<Rental>> GetAllDetailedAsync()
        {
            return await _context.Rentals
                .Include(r => r.Tool)
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Include(r => r.Payments)
                .Include(r => r.Review)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetByUserIdAsync(string userId)
        {
            return await _context.Rentals
                .Include(r => r.Tool)
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Where(r => r.Customer != null && r.Customer.User.Id == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Rental rental)
        {
            if (rental == null) return false;
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Rental>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Rentals
                .Include(r => r.Tool)
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync();
        }


    }
}