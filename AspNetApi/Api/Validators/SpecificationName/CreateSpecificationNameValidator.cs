using Api.Services.Interfaces;
using Api.ViewModels.SpecificationName;
using FluentValidation;

namespace Api.Validators.SpecificationName;

public class CreateSpecificationNameValidator : AbstractValidator<CreateSpecificationNameVm> {
	public CreateSpecificationNameValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(sn => sn.Name)
			.NotEmpty()
				.WithMessage("Name is empty or null")
			.MaximumLength(50)
				.WithMessage("Name is too long");

		RuleFor(sv => sv.CategoryId)
			.MustAsync(existingEntityCheckerService.IsExistsCategoryIdAsync)
				.WithMessage("Category with this id is not exists");
	}
}
