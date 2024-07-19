using Api.ViewModels.Pagination;
using Api.ViewModels.SpecificationName;

namespace Api.Services.ControllerServices.Interfaces;

public interface ISpecificationNamesControllerService {
	Task<IEnumerable<SpecificationNameVm>> GetAllAsync();
	Task<PageVm<SpecificationNameVm>> GetPageAsync(SpecificationNameFilterVm vm);
	Task<SpecificationNameVm?> TryGetByIdAsync(long id);
	Task CreateAsync(CreateSpecificationNameVm vm);
	Task UpdateAsync(UpdateSpecificationNameVm vm);
	Task DeleteIfExistsAsync(long id);
}
