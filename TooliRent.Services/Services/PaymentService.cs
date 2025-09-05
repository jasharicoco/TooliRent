using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;

        public PaymentService(IPaymentRepository repo)
        {
            _repo = repo;
        }
    }
}
