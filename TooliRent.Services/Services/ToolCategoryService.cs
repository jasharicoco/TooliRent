using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class ToolCategoryService : IToolCategoryService
    {
        private readonly IToolCategoryRepository _repo;

        public ToolCategoryService(IToolCategoryRepository repo)
        {
            _repo = repo;
        }
    }
}
