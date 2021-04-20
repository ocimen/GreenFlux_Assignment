using FluentValidation;
using GreenFlux.Api.Models.RequestModels;

namespace GreenFlux.Api.Validators
{
    public class CreateConnectorValidator : AbstractValidator<CreateConnector>
    {
        public CreateConnectorValidator()
        {
            RuleFor(x => x.MaxCurrent).NotNull().NotEmpty().WithMessage("Current can not be null or empty");
            RuleFor(x => x.MaxCurrent).GreaterThan(0).WithMessage("Current must be greater than zero");
        }
    }
}
