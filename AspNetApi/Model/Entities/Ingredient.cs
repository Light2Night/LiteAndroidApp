namespace Model.Entities;

public class Ingredient {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;

	public ICollection<PizzaIngredient> Pizzas { get; set; } = null!;
}
