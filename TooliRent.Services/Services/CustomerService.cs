using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }
    }
}
