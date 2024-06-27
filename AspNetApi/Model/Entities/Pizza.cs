namespace Model.Entities;

public class Pizza {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public string Description { get; set; } = null!;

	public double Rating { get; set; }

	public bool IsAvailable { get; set; }

	public long CategoryId { get; set; }
	public Category Category { get; set; } = null!;

	public ICollection<PizzaImage> Images { get; set; } = null!;
}
