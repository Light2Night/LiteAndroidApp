using Api.ViewModels.Pagination;
using Api.ViewModels.Pizza;

namespace Api.Services.ControllerServices.Interfaces;

public interface IPizzasControllerService {
	Task<IEnumerable<PizzaVm>> GetAllAsync();
	Task<PageVm<PizzaVm>> GetPageAsync(PizzaFilterVm vm);
	Task<PizzaVm?> TryGetByIdAsync(long id);
	Task CreateAsync(CreatePizzaVm vm);
	Task UpdateAsync(UpdatePizzaVm vm);
	Task DeleteIfExistsAsync(long id);
}
