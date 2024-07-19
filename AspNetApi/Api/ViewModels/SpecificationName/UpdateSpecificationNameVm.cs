namespace Api.ViewModels.SpecificationName;

public class UpdateSpecificationNameVm {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public long CategoryId { get; set; }
}
