using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Repositories.Interfaces;

public class ReviewValidator : AbstractValidator<Review>
{
    public ReviewValidator(IReviewRepository reviewRepo, IHttpContextAccessor httpContextAccessor)
    {
        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(r => r.Comment)
            .NotEmpty()
            .MinimumLength(10)
            .WithMessage("Comment must be at least 10 characters long.");

        // Kontrollera att användaren hyrde verktyget och att det inte finns en review redan
        RuleFor(r => r)
            .MustAsync(async (review, cancellation) =>
            {
                var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return false;

                // Hämta hyrning
                var rental = await reviewRepo.GetRentalWithCustomerAsync(review.RentalId, cancellation);
                if (rental == null || rental.Customer.UserId != userId || rental.Status != RentalStatus.Returned)
                    return false;

                // Kontrollera att det inte redan finns en review
                var exists = await reviewRepo.GetByRentalIdAsync(review.RentalId);
                return !exists.Any();
            })
            .WithMessage("You can only review tools you have rented and returned, and only once per rental.");
    }
}
