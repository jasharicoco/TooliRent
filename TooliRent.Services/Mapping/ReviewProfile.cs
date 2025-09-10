using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>();
        }
    }
}
