using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;
using TooliRent.Services.DTOs;
using TooliRent.Services.Services.Interfaces;

namespace TooliRent.Services.Services
{
    public class ReviewService : GenericService<Review, ReviewDto, CreateReviewDto, UpdateReviewDto>, IReviewService
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewService(IReviewRepository reviewRepo, IMapper mapper) : base(reviewRepo, mapper)
        {
            _reviewRepo = reviewRepo;
        }
        public override async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            // Dublettkontroll i service som backup
            var existing = await _reviewRepo.GetByRentalIdAsync(dto.RentalId);
            if (existing.Any())
                throw new InvalidOperationException("A review already exists for this rental.");

            var entity = _mapper.Map<Review>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            await _repository.AddAsync(entity);
            return _mapper.Map<ReviewDto>(entity);
        }

        public async Task<IEnumerable<ReviewDto>> GetByRentalIdAsync(int rentalId)
        {
            var reviews = await _reviewRepo.GetByRentalIdAsync(rentalId);
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }
    }
}
