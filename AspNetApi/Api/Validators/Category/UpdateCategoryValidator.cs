using Api.Services.Interfaces;
using Api.ViewModels.Category;
using FluentValidation;

namespace Api.Validators.Category;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryVm> {
	public UpdateCategoryValidator(IExistingEntityCheckerService existingEntityCheckerService, IImageValidator imageValidator) {
		RuleFor(c => c.Id)
			.MustAsync(existingEntityCheckerService.IsExistsCategoryIdAsync)
				.WithMessage("Category with this id is not exists");

		RuleFor(c => c.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(255)
				.WithMessage("Name is too long");

		RuleFor(c => c.Image)
			.NotNull()
				.WithMessage("Image is not selected")
			.MustAsync(imageValidator.IsValidImageAsync)
				.WithMessage("Image is not valid");
	}
}
