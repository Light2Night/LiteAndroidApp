namespace Api.ViewModels.Ingredient;

public class CreateIngredientVm {
	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}
