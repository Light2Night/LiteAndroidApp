namespace Model.Entities;

public class Category {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Image { get; set; } = null!;

	public ICollection<Pizza> Pizzas { get; set; } = null!;

	public ICollection<SpecificationName> SpecificationNames { get; set; } = null!;
}
