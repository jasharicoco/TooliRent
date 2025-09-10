namespace TooliRent.Services.DTOs
{
    public record ReviewDto(int Id, int RentalId, int Rating, string Comment, DateTime CreatedAt);
    public record CreateReviewDto(int RentalId, int Rating, string Comment);
    public record UpdateReviewDto(int Rating, string Comment);
}
