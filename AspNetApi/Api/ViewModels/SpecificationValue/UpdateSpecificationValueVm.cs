namespace Api.ViewModels.SpecificationValue;

public class UpdateSpecificationValueVm {
	public long Id { get; set; }

	public string Value { get; set; } = null!;

	public long SpecificationNameId { get; set; }
}
