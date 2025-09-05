using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class RentalRepository
        : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context) { }
    }
}
