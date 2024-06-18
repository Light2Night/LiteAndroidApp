using Api.ViewModels.Pagination;

namespace Api.Services.Interfaces;

public interface IPaginationService<EntityVmType, PaginationVmType> where PaginationVmType : PaginationVm {
	Task<PageVm<EntityVmType>> GetPageAsync(PaginationVmType vm);
}
