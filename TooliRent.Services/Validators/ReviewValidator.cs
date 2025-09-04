using FluentValidation;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Services.Validators
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        //public ReviewValidator(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        public ReviewValidator()
        {
            RuleFor(r => r.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5.");

            RuleFor(r => r.Comment)
                .NotEmpty()
                .MinimumLength(10)
                .WithMessage("Comment must be at least 10 characters long.");

            //// Kontrollera att användaren hyrde verktyget
            //RuleFor(r => r)
            //    .MustAsync(async (review, cancellation) =>
            //    {
            //        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //        if (string.IsNullOrEmpty(userId))
            //        {
            //            return false;
            //        }

            //        var rental = await context.Rentals
            //            .Include(r => r.Customer)
            //            .FirstOrDefaultAsync(r => r.Id == review.RentalId, cancellation);

            //        return rental != null && rental.Customer.UserId == userId;
            //    })
            //    .WithMessage("You can only review tools you have rented.");
        }
    }
}
