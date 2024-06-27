namespace Api.Services.Interfaces;

public interface IExistingEntityCheckerService {
	Task<bool> IsExistsCategoryIdAsync(long id, CancellationToken cancellationToken);
	Task<bool> IsExistsIngredientIdAsync(long id, CancellationToken cancellationToken);

}
