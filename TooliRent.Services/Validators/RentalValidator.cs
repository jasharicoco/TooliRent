using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validators
{
    public class CreateRentalDtoValidator : AbstractValidator<CreateRentalDto>
    {
        public CreateRentalDtoValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0);
            RuleFor(x => x.ToolId).GreaterThan(0);

            RuleFor(x => x.StartDate)
                .Must(d => d.Date >= DateTime.UtcNow.Date)
                .WithMessage("StartDate får inte vara bakåt i tiden.");

            RuleFor(x => x.EndDate)
                .Must((dto, end) => end.Date > dto.StartDate.Date)
                .WithMessage("EndDate måste vara efter StartDate (minst 1 dag).");
        }
    }

    public class UpdateRentalDtoValidator : AbstractValidator<UpdateRentalDto>
    {
        public UpdateRentalDtoValidator()
        {
            When(x => x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.EndDate!.Value.Date)
                    .GreaterThan(DateTime.UtcNow.Date.AddDays(-3650)); // sanity check
            });
        }
    }
}
