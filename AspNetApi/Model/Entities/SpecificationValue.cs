namespace Model.Entities;

public class SpecificationValue {
	public long Id { get; set; }

	public string Value { get; set; } = null!;

	public long SpecificationNameId { get; set; }
	public SpecificationName SpecificationName { get; set; } = null!;

	public ICollection<PizzaSpecificationValue> Pizzas { get; set; } = null!;
}
