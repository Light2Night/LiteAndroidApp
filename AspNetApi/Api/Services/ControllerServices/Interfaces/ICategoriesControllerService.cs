using Api.ViewModels.Category;
using Api.ViewModels.Pagination;

namespace Api.Services.ControllerServices.Interfaces;

public interface ICategoriesControllerService {
	Task<IEnumerable<CategoryVm>> GetAllAsync();
	Task<PageVm<CategoryVm>> GetPageAsync(CategoryFilterVm vm);
	Task CreateAsync(CreateCategoryVm vm);
	Task UpdateAsync(UpdateCategoryVm vm);
	Task DeleteIfExistsAsync(long id);
}