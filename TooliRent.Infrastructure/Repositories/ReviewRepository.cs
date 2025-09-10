using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetByRentalIdAsync(int rentalId)
        {
            return await _dbSet
                .Where(r => r.RentalId == rentalId)
                .ToListAsync();
        }
        public async Task<Rental?> GetRentalWithCustomerAsync(int rentalId, CancellationToken cancellationToken = default)
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == rentalId, cancellationToken);
        }

    }
}
