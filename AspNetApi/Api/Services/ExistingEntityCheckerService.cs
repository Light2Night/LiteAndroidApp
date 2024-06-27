using Api.Services.Interfaces;
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
}
