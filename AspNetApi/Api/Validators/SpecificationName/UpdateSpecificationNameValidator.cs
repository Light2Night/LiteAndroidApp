using Api.Services.Interfaces;
using Api.ViewModels.SpecificationName;
using FluentValidation;

namespace Api.Validators.SpecificationName;

public class UpdateSpecificationNameValidator : AbstractValidator<UpdateSpecificationNameVm> {
	public UpdateSpecificationNameValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(sn => sn.Id)
			.MustAsync(existingEntityCheckerService.IsExistsSpecificationNameIdAsync)
				.WithMessage("SpecificationName with this id is not exists");

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
