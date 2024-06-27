namespace Model.Entities;

public class PizzaImage {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public int Priority { get; set; }

	public long PizzaId { get; set; }
	public Pizza Pizza { get; set; } = null!;
}
