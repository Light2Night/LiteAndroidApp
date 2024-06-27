using Api.ViewModels.Ingredient;
using Api.ViewModels.Pagination;

namespace Api.Services.ControllerServices.Interfaces;

public interface IIngredientsControllerService {
	Task<IEnumerable<IngredientVm>> GetAllAsync();
	Task<PageVm<IngredientVm>> GetPageAsync(IngredientFilterVm vm);
	Task<IngredientVm?> TryGetByIdAsync(long id);
	Task CreateAsync(CreateIngredientVm vm);
	Task UpdateAsync(UpdateIngredientVm vm);
	Task DeleteIfExistsAsync(long id);
}
