using Api.Services.PaginationServices.Base;
using Api.ViewModels.SpecificationValue;
using AutoMapper;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class SpecificationValuesPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<SpecificationValue, SpecificationValueVm, SpecificationValueFilterVm>(mapper) {

	protected override IQueryable<SpecificationValue> GetQuery() => context.SpecificationValues.OrderBy(sv => sv.Id);

	protected override IQueryable<SpecificationValue> FilterQuery(IQueryable<SpecificationValue> query, SpecificationValueFilterVm vm) {
		if (vm.Value is not null)
			query = query.Where(sv => sv.Value.ToLower().Contains(vm.Value.ToLower()));

		if (vm.SpecificationNameId is not null)
			query = query.Where(sv => sv.SpecificationNameId == vm.SpecificationNameId);

		return query;
	}
}
