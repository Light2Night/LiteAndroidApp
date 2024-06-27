using Api.ViewModels.Pagination;

namespace Api.ViewModels.Ingredient;

public class IngredientFilterVm : PaginationVm {
	public string? Name { get; set; }
}
