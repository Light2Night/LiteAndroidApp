using Api.ViewModels.Category;

namespace Api.Services.ControllerServices.Interfaces;

public interface ICategoriesControllerService {
	Task<IEnumerable<CategoryVm>> GetAllAsync();
	Task CreateAsync(CreateCategoryVm vm);
	Task UpdateAsync(UpdateCategoryVm vm);
	Task DeleteIfExistsAsync(long id);
}