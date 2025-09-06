using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class ToolService
        : GenericService<Tool, ToolDto, CreateToolDto, UpdateToolDto>, IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly IMapper _mapper;

        public ToolService(IToolRepository toolRepository, IMapper mapper)
            : base(toolRepository, mapper)
        {
            _toolRepository = toolRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RentalDto>> GetRentalsForTool(int toolId)
        {
            var rentals = await _toolRepository.GetRentalsByToolIdAsync(toolId);
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<IEnumerable<ToolDto>> GetFilteredAsync(int? categoryId, string? condition, bool? availableOnly)
        {
            var tools = await _toolRepository.GetAllWithCategoryAndRentalsAsync();

            if (categoryId.HasValue)
                tools = tools.Where(t => t.ToolCategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(condition) && Enum.TryParse<ToolCondition>(condition, true, out var parsedCondition))
                tools = tools.Where(t => t.Condition == parsedCondition);

            if (availableOnly == true)
            {
                tools = tools.Where(t =>
                    !t.Rentals.Any(r => r.EndDate == null || r.EndDate > DateTime.UtcNow));
            }

            return _mapper.Map<IEnumerable<ToolDto>>(tools);
        }
    }
}
