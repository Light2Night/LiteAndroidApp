using Api.DataTransferObjects;

namespace Api.Services.Interfaces;

public interface ICacheService {
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName);
	Task<bool> IsContainsCacheAsync(ActionDto action);
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName, string arguments);
	Task<bool> IsContainsCacheAsync(ActionDto action, string arguments);

	Task<T> GetCacheAsync<T>(string controllerName, string actionName);
	Task<T> GetCacheAsync<T>(ActionDto action);
	Task<T> GetCacheAsync<T>(string controllerName, string actionName, string arguments);
	Task<T> GetCacheAsync<T>(ActionDto action, string arguments);

	Task SetCacheAsync(string controllerName, string actionName, object value, TimeSpan? expiry);
	Task SetCacheAsync(ActionDto action, object value, TimeSpan? expiry);
	Task SetCacheAsync(string controllerName, string actionName, string arguments, object value, TimeSpan? expiry);
	Task SetCacheAsync(ActionDto action, string arguments, object value, TimeSpan? expiry);
}
