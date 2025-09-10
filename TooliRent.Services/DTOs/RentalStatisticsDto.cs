namespace TooliRent.Services.DTOs
{
    public record RentalStatisticsDto(int TotalRentals, int ActiveRentals, int OverdueRentals, int ReturnedRentals, string MostPopularTool, double AverageRentalDays);
}
