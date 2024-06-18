using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Context;

namespace Api.Services;

public class ExistingEntityCheckerService(
	DataContext context
) : IExistingEntityCheckerService {

	public async Task<bool> IsExistsCategoryIdAsync(long id, CancellationToken cancellationToken) =>
		await context.Categories.AnyAsync(c => c.Id == id, cancellationToken);

}
