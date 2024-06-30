using Api.ViewModels.PizzaSize;

namespace Api.Services.ControllerServices.Interfaces;

public interface IPizzaSizesControllerService {
	Task CreateAsync(CreatePizzaSizeVm vm);
	Task UpdateAsync(UpdatePizzaSizeVm vm);
	Task DeleteIfExistsAsync(long pizzaId, long sizeId);
}
