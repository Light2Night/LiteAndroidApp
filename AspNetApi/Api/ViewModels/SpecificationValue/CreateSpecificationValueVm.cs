namespace Api.ViewModels.SpecificationValue;

public class CreateSpecificationValueVm {
	public string Value { get; set; } = null!;

	public long SpecificationNameId { get; set; }
}
