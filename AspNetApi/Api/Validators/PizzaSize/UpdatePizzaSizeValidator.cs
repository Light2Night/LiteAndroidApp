using Api.Services.Interfaces;
using Api.ViewModels.PizzaSize;
using FluentValidation;

namespace Api.Validators.PizzaSize;

public class UpdatePizzaSizeValidator : AbstractValidator<UpdatePizzaSizeVm> {
	public UpdatePizzaSizeValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(ps => ps)
			.MustAsync(
				async (ps, cancellationToken) =>
					await existingEntityCheckerService.IsExistsPizzaSizeKeyAsync(ps.PizzaId, ps.SizeId, cancellationToken)
			)
				.WithMessage("Size with this PizzaId and SizeId is not exists");

		RuleFor(ps => ps.Price)
			.GreaterThanOrEqualTo(0)
				.WithMessage("The price can't be a negative")
			.Must(price => decimal.Round(price, 2) == price)
				.WithMessage("Can only be up to two decimal places");
	}
}
