using Api.ViewModels.Size;
using FluentValidation;

namespace Api.Validators.Size;

public class CreateSizeValidator : AbstractValidator<CreateSizeVm> {
	public CreateSizeValidator() {
		RuleFor(s => s.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(50)
				.WithMessage("Name is too long");
	}
}
