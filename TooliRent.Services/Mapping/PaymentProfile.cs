using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            // Payment → PaymentDto
            CreateMap<Payment, PaymentDto>();

            // CreatePaymentDto → Payment
            CreateMap<CreatePaymentDto, Payment>();

            // UpdatePaymentDto → Payment
            CreateMap<UpdatePaymentDto, Payment>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
