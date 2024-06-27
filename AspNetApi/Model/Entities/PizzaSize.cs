namespace Model.Entities;

public class PizzaSize {
	public long PizzaId { get; set; }
	public Pizza Pizza { get; set; } = null!;

	public long SizeId { get; set; }
	public Size Size { get; set; } = null!;

	public decimal Price { get; set; }
}
