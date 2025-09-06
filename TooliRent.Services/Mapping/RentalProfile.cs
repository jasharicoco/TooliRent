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


            CreateMap<CreateRentalDto, Rental>(); // värden som sätts i service (TotalPrice, Status, timestamps)
            CreateMap<UpdateRentalDto, Rental>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
