using Api.ViewModels.Pagination;
using Api.ViewModels.Size;

namespace Api.Services.ControllerServices.Interfaces;

public interface ISizesControllerService {
	Task<IEnumerable<SizeVm>> GetAllAsync();
	Task<PageVm<SizeVm>> GetPageAsync(SizeFilterVm vm);
	Task<SizeVm?> TryGetByIdAsync(long id);
	Task CreateAsync(CreateSizeVm vm);
	Task UpdateAsync(UpdateSizeVm vm);
	Task DeleteIfExistsAsync(long id);
}
