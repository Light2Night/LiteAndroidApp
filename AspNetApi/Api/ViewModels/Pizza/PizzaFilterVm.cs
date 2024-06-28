using Api.ViewModels.Pagination;

namespace Api.ViewModels.Pizza;

public class PizzaFilterVm : PaginationVm {
	public string? Name { get; set; }

	public string? Description { get; set; }

	public double? Rating { get; set; }
	public double? MinRating { get; set; }
	public double? MaxRating { get; set; }

	public bool? IsAvailable { get; set; }

	public long? CategoryId { get; set; }

	public IEnumerable<long>? IngredientIds { get; set; }
}
