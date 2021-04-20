using FluentValidation;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Api.Validators
{
    public class CreateGroupValidator : AbstractValidator<CreateGroup>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("The name can not be null");
            RuleFor(x => x.Capacity).NotNull().NotEmpty().WithMessage("The name can not be null");
            RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("The capacity must be greater than zero.");
        }
    }
}
