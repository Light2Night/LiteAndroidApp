using Api.ViewModels.Pagination;

namespace Api.ViewModels.SpecificationValue;

public class SpecificationValueFilterVm : PaginationVm {
	public string? Value { get; set; }

	public long? SpecificationNameId { get; set; }
}
