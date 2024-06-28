namespace Api.ViewModels.Pizza;

public class UpdatePizzaVm {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public double Rating { get; set; }

	public bool IsAvailable { get; set; }

	public long CategoryId { get; set; }

	public IEnumerable<IFormFile> Images { get; set; } = null!;

	public IEnumerable<long>? IngredientIds { get; set; }
}
