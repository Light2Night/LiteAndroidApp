namespace Model.Entities;

public class PizzaSpecificationValue {
	public long PizzaId { get; set; }
	public Pizza Pizza { get; set; } = null!;

	public long SpecificationValueId { get; set; }
	public SpecificationValue SpecificationValue { get; set; } = null!;
}