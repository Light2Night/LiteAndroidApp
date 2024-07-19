using Api.ViewModels.Pagination;
using Api.ViewModels.SpecificationValue;

namespace Api.Services.ControllerServices.Interfaces;

public interface ISpecificationValuesControllerService {
	Task<IEnumerable<SpecificationValueVm>> GetAllAsync();
	Task<PageVm<SpecificationValueVm>> GetPageAsync(SpecificationValueFilterVm vm);
	Task<SpecificationValueVm?> TryGetByIdAsync(long id);
	Task CreateAsync(CreateSpecificationValueVm vm);
	Task UpdateAsync(UpdateSpecificationValueVm vm);
	Task DeleteIfExistsAsync(long id);
}
