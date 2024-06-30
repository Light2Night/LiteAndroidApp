using Api.Services.Interfaces;
using Api.ViewModels.PizzaSize;
using FluentValidation;

namespace Api.Validators.PizzaSize;

public class CreatePizzaSizeValidator : AbstractValidator<CreatePizzaSizeVm> {
	public CreatePizzaSizeValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(ps => ps)
			.MustAsync(
				async (ps, cancellationToken) =>
					!await existingEntityCheckerService.IsExistsPizzaSizeKeyAsync(ps.PizzaId, ps.SizeId, cancellationToken)
			)
				.WithMessage("There is a size with this PizzaId and SizeId");

		RuleFor(ps => ps.Price)
			.GreaterThanOrEqualTo(0)
				.WithMessage("The price can't be a negative")
			.Must(price => decimal.Round(price, 2) == price)
				.WithMessage("Can only be up to two decimal places");
	}
}
