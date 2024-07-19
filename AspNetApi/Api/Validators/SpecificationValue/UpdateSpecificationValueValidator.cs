using Api.Services.Interfaces;
using Api.ViewModels.SpecificationValue;
using FluentValidation;

namespace Api.Validators.SpecificationValue;

public class UpdateSpecificationValueValidator : AbstractValidator<UpdateSpecificationValueVm> {
	public UpdateSpecificationValueValidator(IExistingEntityCheckerService existingEntityCheckerService) {
		RuleFor(sv => sv.Id)
			.MustAsync(existingEntityCheckerService.IsExistsSpecificationValueIdAsync)
				.WithMessage("SpecificationValue with this id is not exists");

		RuleFor(sv => sv.Value)
			.NotEmpty()
				.WithMessage("Value is empty or null")
			.MaximumLength(50)
				.WithMessage("Value is too long");

		RuleFor(sv => sv.SpecificationNameId)
			.MustAsync(existingEntityCheckerService.IsExistsSpecificationNameIdAsync)
				.WithMessage("SpecificationName with this id is not exists");
	}
}
