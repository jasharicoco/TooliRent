using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validators
{
    public class CreateRentalDtoValidator : AbstractValidator<CreateRentalDto>
    {
        public class CreateRentalByUserDtoValidator : AbstractValidator<CreateRentalByUserDto>
        {
            public CreateRentalByUserDtoValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .WithMessage("UserId är obligatoriskt.")
                    .Must(BeValidGuid)
                    .WithMessage("UserId måste vara ett giltigt GUID.");

                RuleFor(x => x.ToolId).GreaterThan(0);

                RuleFor(x => x.StartDate)
                    .Must(d => d.Date >= DateTime.UtcNow.Date)
                    .WithMessage("StartDate får inte vara bakåt i tiden.");

                RuleFor(x => x.EndDate)
                    .Must((dto, end) => end.Date > dto.StartDate.Date)
                    .WithMessage("EndDate måste vara efter StartDate (minst 1 dag).");
            }

            private bool BeValidGuid(string guid) => Guid.TryParse(guid, out _);
        }

        public class CreateRentalByCustomerDtoValidator : AbstractValidator<CreateRentalByCustomerDto>
        {
            public CreateRentalByCustomerDtoValidator()
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
}