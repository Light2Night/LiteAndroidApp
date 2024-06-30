namespace Api.Services.Interfaces;

public interface IExistingEntityCheckerService {
	Task<bool> IsExistsCategoryIdAsync(long id, CancellationToken cancellationToken);

	Task<bool> IsExistsIngredientIdAsync(long id, CancellationToken cancellationToken);
	Task<bool> IsExistsIngredientIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken);
	Task<bool> IsExistsNullPossibleIngredientIdsAsync(IEnumerable<long>? ids, CancellationToken cancellationToken);

	Task<bool> IsExistsPizzaIdAsync(long id, CancellationToken cancellationToken);

	Task<bool> IsExistsSizeIdAsync(long id, CancellationToken cancellationToken);

	Task<bool> IsExistsPizzaSizeKeyAsync(long pizzaId, long sizeId, CancellationToken cancellationToken);
}
