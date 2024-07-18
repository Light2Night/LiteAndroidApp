using Api.ViewModels.Ingredient;
using Api.ViewModels.PizzaImage;
using Api.ViewModels.PizzaSize;
using Api.ViewModels.SpecificationValue;

namespace Api.ViewModels.Pizza;

public class PizzaVm {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public double Rating { get; set; }

	public bool IsAvailable { get; set; }

	public long CategoryId { get; set; }

	public IEnumerable<PizzaImageShortVm> Images { get; set; } = null!;

	public IEnumerable<IngredientVm> Ingredients { get; set; } = null!;

	public IEnumerable<PizzaSizeShortVm> Sizes { get; set; } = null!;

	public IEnumerable<SpecificationValueVm> SpecificationValues { get; set; } = null!;
}
