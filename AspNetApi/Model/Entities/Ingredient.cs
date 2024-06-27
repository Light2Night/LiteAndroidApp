namespace Model.Entities;

public class Ingredient {
	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;

	public ICollection<PizzaIngredient> Pizzas { get; set; } = null!;
}
