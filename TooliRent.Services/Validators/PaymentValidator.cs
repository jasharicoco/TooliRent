using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validation
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.RentalId).GreaterThan(0);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Method).NotEmpty();
        }
    }
}
