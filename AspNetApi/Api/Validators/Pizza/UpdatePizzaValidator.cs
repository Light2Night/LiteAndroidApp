using Api.Services.Interfaces;
using Api.ViewModels.Pizza;
using FluentValidation;

namespace Api.Validators.Pizza;

public class UpdatePizzaValidator : AbstractValidator<UpdatePizzaVm> {
	public UpdatePizzaValidator(IImageValidator imageValidator, IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(p => p.Id)
			.MustAsync(existingEntityCheckerService.IsExistsPizzaIdAsync)
				.WithMessage("Pizza with this id is not exists");

		RuleFor(p => p.Name)
			.NotEmpty()
				.WithMessage("Name is empty")
			.MaximumLength(255)
				.WithMessage("Name is too long");

		RuleFor(p => p.Description)
			.NotEmpty()
				.WithMessage("Description is empty")
			.MaximumLength(4000)
				.WithMessage("Description is too long (4000)");

		RuleFor(p => p.Rating)
			.InclusiveBetween(1, 5)
				.WithMessage("Rating must be in the range from 1 to 5");

		RuleFor(p => p.CategoryId)
			.MustAsync(existingEntityCheckerService.IsExistsCategoryIdAsync)
				.WithMessage("Category with this id is not exists");

		RuleFor(p => p.Images)
			.MustAsync(imageValidator.IsValidImagesAsync)
				.WithMessage("One ore more of images are invalid");

		RuleFor(p => p.IngredientIds)
			.MustAsync(existingEntityCheckerService.IsExistsNullPossibleIngredientIdsAsync)
				.WithMessage("IngredientIds contains the Id of a non-existing element");

		RuleFor(p => p.SpecificationValueIds)
			.MustAsync(existingEntityCheckerService.IsExistsNullPossibleSpecificationValueIdsAsync)
				.WithMessage("SpecificationValueIds contains the Id of a non-existing element");
	}
}
