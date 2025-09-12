using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class RentalProfile : Profile
    {
        public RentalProfile()
        {
            CreateMap<Rental, RentalDto>()
                .ForMember(d => d.ToolName, o => o.MapFrom(s => s.Tool.Name))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.User.FirstName + " " + s.Customer.User.LastName))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<CreateRentalDto, Rental>()
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) // Ignore CustomerId - set manually in service
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore()) // Ignore TotalPrice - calculated in service
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Ignore Status - set manually in service
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Ignore CreatedAt - set manually in service
                .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore()); // Ignore ModifiedAt - set manually in service

            CreateMap<UpdateRentalDto, Rental>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
