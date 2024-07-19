using Api.ViewModels.Pagination;

namespace Api.ViewModels.SpecificationName;

public class SpecificationNameFilterVm : PaginationVm {
	public string? Name { get; set; }

	public long? CategoryId { get; set; }
}
