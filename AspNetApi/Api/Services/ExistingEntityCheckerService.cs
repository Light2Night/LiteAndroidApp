﻿using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Context;

namespace Api.Services;

public class ExistingEntityCheckerService(
	DataContext context
) : IExistingEntityCheckerService {

	public async Task<bool> IsExistsCategoryIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Categories.AnyAsync(c => c.Id == id, cancellationToken);

	public async Task<bool> IsExistsIngredientIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Ingredients.AnyAsync(i => i.Id == id, cancellationToken);

	public async Task<bool> IsExistsIngredientIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken) {
		var ingredientsFromDb = await context.Ingredients
			.Where(i => ids.Contains(i.Id))
			.Select(i => i.Id)
			.ToArrayAsync();

		return ids.All(id => ingredientsFromDb.Contains(id));
	}

	public async Task<bool> IsExistsNullPossibleIngredientIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken) {
		if (ids is null)
			return true;

		return await IsExistsIngredientIdsAsync(ids, cancellationToken);
	}

	public async Task<bool> IsExistsPizzaIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Pizzas.AnyAsync(p => p.Id == id, cancellationToken);

	public async Task<bool> IsExistsSizeIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Sizes.AnyAsync(s => s.Id == id, cancellationToken);
}
