using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class ToolCategoryProfile : Profile
    {
        public ToolCategoryProfile()
        {
            CreateMap<ToolCategory, ToolCategoryDto>();
            CreateMap<CreateToolCategoryDto, ToolCategory>();
            CreateMap<UpdateToolCategoryDto, ToolCategory>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
