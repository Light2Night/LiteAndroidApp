namespace Model.Entities;

public class Size {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public ICollection<PizzaSize> Pizzas { get; set; } = null!;
}
