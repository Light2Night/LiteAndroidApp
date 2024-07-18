using Api.Services.PaginationServices.Base;
using Api.ViewModels.Pizza;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entities;

namespace Api.Services.PaginationServices;

public class PizzasPaginationService(
	DataContext context,
	IMapper mapper
) : PaginationService<Pizza, PizzaVm, PizzaFilterVm>(mapper) {

	protected override IQueryable<Pizza> GetQuery() => context.Pizzas
		.OrderBy(i => i.Id)
		.AsSplitQuery();

	protected override IQueryable<Pizza> FilterQuery(IQueryable<Pizza> query, PizzaFilterVm vm) {
		if (vm.Name is not null)
			query = query.Where(p => p.Name.ToLower().Contains(vm.Name.ToLower()));

		if (vm.Description is not null)
			query = query.Where(p => p.Description.ToLower().Contains(vm.Description.ToLower()));

		if (vm.Rating is not null)
			query = query.Where(p => p.Rating == vm.Rating);
		if (vm.MinRating is not null)
			query = query.Where(p => p.Rating >= vm.MinRating);
		if (vm.MaxRating is not null)
			query = query.Where(p => p.Rating <= vm.MaxRating);

		if (vm.IsAvailable is not null)
			query = query.Where(p => p.IsAvailable == vm.IsAvailable);

		if (vm.CategoryId is not null)
			query = query.Where(p => p.CategoryId == vm.CategoryId);

		if (vm.IngredientIds is not null)
			query = query.Where(
				p => vm.IngredientIds.All(
					iId => p.Ingredients.Any(pi => pi.IngredientId == iId)
				)
			);

		if (vm.SpecificationValueIds is not null) {
			var searchedSpecificationNames = context.SpecificationNames
				.Include(sn => sn.Values)
				.Where(
					sn => sn.Values
						.Any(sv => vm.SpecificationValueIds.Contains(sv.Id))
				)
				.ToArray();

			foreach (var specificationName in searchedSpecificationNames) {
				var searchedSpecificationValueIds = vm.SpecificationValueIds
					.Intersect(specificationName.Values.Select(sv => sv.Id))
					.ToArray();

				query = query
					.Include(p => p.SpecificationValues)
						.ThenInclude(psv => psv.SpecificationValue)
							.ThenInclude(sv => sv.SpecificationName)
					.Where(
						p => p.SpecificationValues
							.Select(psv => psv.SpecificationValueId)
							.Any(svId => searchedSpecificationValueIds.Contains(svId))
					);
			}
		}

		return query;
	}
}
