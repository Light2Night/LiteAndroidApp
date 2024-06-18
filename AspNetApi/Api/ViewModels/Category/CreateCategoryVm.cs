namespace Api.ViewModels.Category;

public class CreateCategoryVm {
	public string Name { get; set; } = null!;

	public IFormFile Image { get; set; } = null!;
}
