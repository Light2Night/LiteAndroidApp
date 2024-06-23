using Api.DataTransferObjects;
using Api.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Api.Services;

public class RedisCacheService(
	IConnectionMultiplexer connectionMultiplexer
) : ICacheService {
	private readonly IDatabase _redis = connectionMultiplexer.GetDatabase();

	public async Task<bool> IsContainsCacheAsync(string controllerName, string actionName) {
		return await IsContainsCacheByKeyAsync(CreateKey(controllerName, actionName));
	}

	public async Task<bool> IsContainsCacheAsync(ActionDto action) {
		return await IsContainsCacheByKeyAsync(CreateKey(action));
	}

	public async Task<bool> IsContainsCacheAsync(string controllerName, string actionName, string arguments) {
		return await IsContainsCacheByKeyAsync(CreateKey(controllerName, actionName, arguments));
	}

	public async Task<bool> IsContainsCacheAsync(ActionDto action, string arguments) {
		return await IsContainsCacheByKeyAsync(CreateKey(action, arguments));
	}


	public async Task<T> GetCacheAsync<T>(string controllerName, string actionName) {
		return await GetCacheByKeyAsync<T>(CreateKey(controllerName, actionName));
	}

	public async Task<T> GetCacheAsync<T>(ActionDto action) {
		return await GetCacheByKeyAsync<T>(CreateKey(action));
	}

	public async Task<T> GetCacheAsync<T>(string controllerName, string actionName, string arguments) {
		return await GetCacheByKeyAsync<T>(CreateKey(controllerName, actionName, arguments));
	}

	public async Task<T> GetCacheAsync<T>(ActionDto action, string arguments) {
		return await GetCacheByKeyAsync<T>(CreateKey(action, arguments));
	}


	public async Task SetCacheAsync(string controllerName, string actionName, object value, TimeSpan? expiry) {
		await SetCacheByKeyAsync(CreateKey(controllerName, actionName), value, expiry);
	}

	public async Task SetCacheAsync(ActionDto action, object value, TimeSpan? expiry) {
		await SetCacheByKeyAsync(CreateKey(action), value, expiry);
	}

	public async Task SetCacheAsync(string controllerName, string actionName, string arguments, object value, TimeSpan? expiry) {
		await SetCacheByKeyAsync(CreateKey(controllerName, actionName, arguments), value, expiry);
	}

	public async Task SetCacheAsync(ActionDto action, string arguments, object value, TimeSpan? expiry) {
		await SetCacheByKeyAsync(CreateKey(action, arguments), value, expiry);
	}



	private async Task<bool> IsContainsCacheByKeyAsync(string key) {
		RedisValue json = await _redis.StringGetAsync(key);

		return !json.IsNullOrEmpty;
	}

	private async Task<T> GetCacheByKeyAsync<T>(string key) {
		RedisValue json = await _redis.StringGetAsync(key);

		var value = JsonSerializer.Deserialize<T>(json)
			?? throw new NullReferenceException("JsonSerializer.Deserialize returns null");

		return value;
	}

	private async Task SetCacheByKeyAsync(string key, object value, TimeSpan? expiry) {
		var json = JsonSerializer.Serialize(value);

		await _redis.StringSetAsync(key, json);

		if (expiry is not null)
			await _redis.KeyExpireAsync(key, expiry);
	}

	private static string CreateKey(string controllerName, string actionName) => $"{controllerName}:{actionName}";
	private static string CreateKey(ActionDto action) => CreateKey(action.ControllerName, action.ActionName);
	private static string CreateKey(string controllerName, string actionName, string arguments) {
		var argumentsJson = JsonSerializer.Serialize(arguments);

		return $"{CreateKey(controllerName, actionName)}:{arguments}";
	}
	private static string CreateKey(ActionDto action, string arguments) =>
		CreateKey(action.ControllerName, action.ActionName, arguments);
}
