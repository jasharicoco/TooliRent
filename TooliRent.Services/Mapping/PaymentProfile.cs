using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .ForMember(d => d.Method, o => o.MapFrom(s => s.PaymentMethod.ToString()))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(d => d.PaymentMethod, o => o.MapFrom(s => Enum.Parse<PaymentMethod>(s.Method, true)));

            CreateMap<UpdatePaymentDto, Payment>()
                .ForMember(d => d.PaymentMethod, o => o.MapFrom((s, d) =>
                    s.Method != null ? Enum.Parse<PaymentMethod>(s.Method, true) : d.PaymentMethod))
                .ForMember(d => d.Status, o => o.MapFrom((s, d) =>
                    s.Status != null ? Enum.Parse<PaymentStatus>(s.Status, true) : d.Status))
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}