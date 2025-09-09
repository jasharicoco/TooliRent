using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class ToolCategoryService
        : GenericService<ToolCategory, ToolCategoryDto, CreateToolCategoryDto, UpdateToolCategoryDto>,
          IToolCategoryService
    {
        public ToolCategoryService(IToolCategoryRepository repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
    }
}
