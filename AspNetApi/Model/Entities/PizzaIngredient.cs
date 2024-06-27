namespace Model.Entities;

public class PizzaIngredient {
	public long PizzaId { get; set; }
	public Pizza Pizza { get; set; } = null!;

	public long IngredientId { get; set; }
	public Ingredient Ingredient { get; set; } = null!;
}
