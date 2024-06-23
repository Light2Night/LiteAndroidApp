using Api.DataTransferObjects;

namespace Api.Services.Interfaces;

public interface ICacheService {
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName);
	Task<bool> IsContainsCacheAsync(ActionDto action);
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName, object argument);
	Task<bool> IsContainsCacheAsync(ActionDto action, object argument);

	Task<T> GetCacheAsync<T>(string controllerName, string actionName);
	Task<T> GetCacheAsync<T>(ActionDto action);
	Task<T> GetCacheAsync<T>(string controllerName, string actionName, object argument);
	Task<T> GetCacheAsync<T>(ActionDto action, object argument);

	Task SetCacheAsync(string controllerName, string actionName, object? value, TimeSpan? expiry = null);
	Task SetCacheAsync(ActionDto action, object? value, TimeSpan? expiry = null);
	Task SetCacheAsync(string controllerName, string actionName, object argument, object? value, TimeSpan? expiry = null);
	Task SetCacheAsync(ActionDto action, object argument, object? value, TimeSpan? expiry = null);

	Task DeleteCacheByControllerAsync(string controllerName);
	Task DeleteCacheByControllerAsync(ControllerDto controller);

	Task DeleteCacheByActionAsync(string controllerName, object actionName);
	Task DeleteCacheByActionAsync(ActionDto action);

	Task DeleteCacheByArgumentAsync(string controllerName, string actionName, object argument);
	Task DeleteCacheByArgumentAsync(ActionDto action, object argument);
}
