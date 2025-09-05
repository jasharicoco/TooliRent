using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class PaymentRepository
        : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }
    }
}
