using Api.Services.PaginationServices.Base;
using Api.ViewModels.SpecificationName;
using AutoMapper;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class SpecificationNamesPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<SpecificationName, SpecificationNameVm, SpecificationNameFilterVm>(mapper) {

	protected override IQueryable<SpecificationName> GetQuery() => context.SpecificationNames.OrderBy(sv => sv.Id);

	protected override IQueryable<SpecificationName> FilterQuery(IQueryable<SpecificationName> query, SpecificationNameFilterVm vm) {
		if (vm.Name is not null)
			query = query.Where(sn => sn.Name.ToLower().Contains(vm.Name.ToLower()));

		if (vm.CategoryId is not null)
			query = query.Where(sv => sv.CategoryId == vm.CategoryId);

		return query;
	}
}
