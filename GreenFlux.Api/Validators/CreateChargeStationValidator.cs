using FluentValidation;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Api.Validators
{
    public class CreateChargeStationValidator : AbstractValidator<CreateChargeStation>
    {
        public CreateChargeStationValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("The name can not be null or empty");
            RuleFor(x => x.Connectors.Count).GreaterThan(0).WithMessage("Charge station has to have at least one connector");
        }
    }
}
