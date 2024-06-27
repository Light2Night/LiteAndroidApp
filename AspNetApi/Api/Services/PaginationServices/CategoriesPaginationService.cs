using Api.Services.PaginationServices.Base;
using Api.ViewModels.Category;
using AutoMapper;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class CategoriesPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<Category, CategoryVm, CategoryFilterVm>(mapper) {

	protected override IQueryable<Category> GetQuery() => context.Categories.OrderBy(c => c.Id);

	protected override IQueryable<Category> FilterQuery(IQueryable<Category> query, CategoryFilterVm paginationVm) {
		if (paginationVm.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(paginationVm.Name.ToLower()));

		return query;
	}
}
