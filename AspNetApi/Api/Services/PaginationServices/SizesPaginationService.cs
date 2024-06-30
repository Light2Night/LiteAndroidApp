using Api.Services.PaginationServices.Base;
using Api.ViewModels.Size;
using AutoMapper;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class SizesPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<Size, SizeVm, SizeFilterVm>(mapper) {

	protected override IQueryable<Size> GetQuery() => context.Sizes.OrderBy(s => s.Id);

	protected override IQueryable<Size> FilterQuery(IQueryable<Size> query, SizeFilterVm vm) {
		if (vm.Name is not null)
			query = query.Where(s => s.Name.ToLower().Contains(vm.Name.ToLower()));

		return query;
	}
}
