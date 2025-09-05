using FluentValidation;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Validators
{
    public class CreateToolDtoValidator : AbstractValidator<CreateToolDto>
    {
        public CreateToolDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.ToolCategoryId).GreaterThan(0);
        }
    }

    public class UpdateToolDtoValidator : AbstractValidator<UpdateToolDto>
    {
        public UpdateToolDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.ToolCategoryId).GreaterThan(0);
        }
    }
}
