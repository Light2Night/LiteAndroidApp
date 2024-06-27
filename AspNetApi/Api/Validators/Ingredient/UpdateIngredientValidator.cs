using Api.Services.Interfaces;
using Api.ViewModels.Ingredient;
using FluentValidation;

namespace Api.Validators.Ingredient;

public class UpdateIngredientValidator : AbstractValidator<UpdateIngredientVm> {
	public UpdateIngredientValidator(IImageValidator imageValidator, IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(i => i.Id)
			.MustAsync(existingEntityCheckerService.IsExistsIngredientIdAsync)
				.WithMessage("Ingredient with this id is not exists");

		RuleFor(i => i.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");

		RuleFor(i => i.Image)
			.NotNull()
				.WithMessage("Image is not selected")
			.MustAsync(imageValidator.IsValidImageAsync)
				.WithMessage("Image is not valid");
	}
}
