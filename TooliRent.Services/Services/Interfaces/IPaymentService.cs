using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services.Interfaces
{
    public interface IPaymentService : IGenericService<Payment, PaymentDto, CreatePaymentDto, UpdatePaymentDto>
    {
        Task<PaymentDto?> UpdateAsync(int id, UpdatePaymentDto dto);
        //Task<PaymentDto?> UpdateStatusAsync(int id, PaymentStatus status);
        //Task<PaymentDto?> UpdateMethodAsync(int id, PaymentMethod method);
    }
}
