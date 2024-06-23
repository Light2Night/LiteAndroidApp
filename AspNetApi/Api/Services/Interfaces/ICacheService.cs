namespace Api.Services.Interfaces;

public interface ICacheService {
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName);
	Task<bool> IsContainsCacheAsync(string controllerName, string actionName, string arguments);

	Task<T> GetCacheAsync<T>(string controllerName, string actionName);
	Task<T> GetCacheAsync<T>(string controllerName, string actionName, string arguments);

	Task SetCacheAsync(string controllerName, string actionName, object value, TimeSpan? expiry);
	Task SetCacheAsync(string controllerName, string actionName, string arguments, object value, TimeSpan? expiry);
}
