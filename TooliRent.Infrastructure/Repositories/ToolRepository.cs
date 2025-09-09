using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class ToolRepository : GenericRepository<Tool>, IToolRepository
    {
        public ToolRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Tool>> GetAllWithCategoryAndRentalsAsync()
        {
            return await _context.Tools
                .Include(t => t.ToolCategory)
                .Include(t => t.Rentals)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByToolIdAsync(int toolId)
        {
            return await _context.Rentals
                .Where(r => r.ToolId == toolId)
                .Include(r => r.Tool)
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }


        public override async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools
                .Include(t => t.ToolCategory)
                .ToListAsync();
        }

        public override async Task<Tool?> GetByIdAsync(int id)
        {
            return await _context.Tools
                .Include(t => t.ToolCategory)
                .Include(t => t.Rentals)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
