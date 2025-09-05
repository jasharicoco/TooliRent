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
    }
}
