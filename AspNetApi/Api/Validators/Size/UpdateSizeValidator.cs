using Api.Services.Interfaces;
using Api.ViewModels.Size;
using FluentValidation;

namespace Api.Validators.Size;

public class UpdateSizeValidator : AbstractValidator<UpdateSizeVm> {
	public UpdateSizeValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(s => s.Id)
			.MustAsync(existingEntityCheckerService.IsExistsSizeIdAsync)
				.WithMessage("Size with this id is not exists");

		RuleFor(s => s.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(50)
				.WithMessage("Name is too long");
	}
}
