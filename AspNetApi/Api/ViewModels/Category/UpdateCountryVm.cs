namespace Api.ViewModels.Category;

public class UpdateCategoryVm {
	public long Id { get; set; }

	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}