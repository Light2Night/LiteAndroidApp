namespace Api.ViewModels.Ingredient;

public class UpdateIngredientVm {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}
