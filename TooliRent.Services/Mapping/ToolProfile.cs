using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class ToolProfile : Profile
    {
        public ToolProfile()
        {
            CreateMap<Tool, ToolDto>()
                .ForMember(dest => dest.ToolCategoryName,
                           opt => opt.MapFrom(src => src.ToolCategory.Name))
                .ForMember(dest => dest.Condition,
                           opt => opt.MapFrom(src => src.Condition.ToString()));

            CreateMap<CreateToolDto, Tool>();
            CreateMap<UpdateToolDto, Tool>();
        }
    }

}
