using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories.Interfaces;

namespace TooliRent.Infrastructure.Repositories
{
    public class ToolCategoryRepository
        : GenericRepository<ToolCategory>, IToolCategoryRepository
    {
        public ToolCategoryRepository(AppDbContext context) : base(context) { }
    }
}
