namespace Model.Entities;

public class SpecificationName {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public long CategoryId { get; set; }
	public Category Category { get; set; } = null!;

	public ICollection<SpecificationValue> Values { get; set; } = null!;
}
