using Api.Services.PaginationServices.Base;
using Api.ViewModels.Size;
using Api.ViewModels.SpecificationValue;
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
