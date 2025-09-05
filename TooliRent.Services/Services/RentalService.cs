using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repo;

        public RentalService(IRentalRepository repo)
        {
            _repo = repo;
        }
    }
}
