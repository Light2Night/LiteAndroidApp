using Api.Services.PaginationServices.Base;
using Api.ViewModels.Ingredient;
using AutoMapper;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class IngredientsPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<Ingredient, IngredientVm, IngredientFilterVm>(mapper) {

	protected override IQueryable<Ingredient> GetQuery() => context.Ingredients.OrderBy(i => i.Id);

	protected override IQueryable<Ingredient> FilterQuery(IQueryable<Ingredient> query, IngredientFilterVm paginationVm) {
		if (paginationVm.Name is not null)
			query = query.Where(c => c.Name.ToLower().Contains(paginationVm.Name.ToLower()));

		return query;
	}
}
