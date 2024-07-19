using Api.DataTransferObjects;
using Api.Exceptions;
using Api.Services.Interfaces;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
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
	public async Task<bool> IsContainsCacheAsync(ActionDto action) =>
		await IsContainsCacheAsync(action.ControllerName, action.ActionName);

	public async Task<bool> IsContainsCacheAsync(string controllerName, string actionName, object argument) {
		return await IsContainsCacheByKeyAsync(CreateKey(controllerName, actionName, argument));
	}
	public async Task<bool> IsContainsCacheAsync(ActionDto action, object argument) =>
		await IsContainsCacheAsync(action.ControllerName, action.ActionName, argument);


	public async Task<T> GetCacheAsync<T>(string controllerName, string actionName) {
		return await GetCacheByKeyAsync<T>(CreateKey(controllerName, actionName));
	}
	public async Task<T> GetCacheAsync<T>(ActionDto action) =>
		await GetCacheAsync<T>(action.ControllerName, action.ActionName);

	public async Task<T> GetCacheAsync<T>(string controllerName, string actionName, object argument) {
		return await GetCacheByKeyAsync<T>(CreateKey(controllerName, actionName, argument));
	}
	public async Task<T> GetCacheAsync<T>(ActionDto action, object argument) =>
		await GetCacheAsync<T>(action.ControllerName, action.ActionName, argument);


	public async Task<T?> TryGetCacheAsync<T>(string controllerName, string actionName) {
		return await TryGetCacheByKeyAsync<T>(CreateKey(controllerName, actionName));
	}
	public async Task<T?> TryGetCacheAsync<T>(ActionDto action) =>
		await TryGetCacheAsync<T>(action.ControllerName, action.ActionName);

	public async Task<T?> TryGetCacheAsync<T>(string controllerName, string actionName, object argument) {
		return await TryGetCacheByKeyAsync<T>(CreateKey(controllerName, actionName, argument));
	}
	public async Task<T?> TryGetCacheAsync<T>(ActionDto action, object argument) =>
		await TryGetCacheAsync<T>(action.ControllerName, action.ActionName, argument);


	public async Task SetCacheAsync(string controllerName, string actionName, object? value, TimeSpan? expiry = null) {
		await SetCacheByKeyAsync(CreateKey(controllerName, actionName), value, expiry);
	}
	public async Task SetCacheAsync(ActionDto action, object? value, TimeSpan? expiry = null) =>
		await SetCacheAsync(action.ControllerName, action.ActionName, value, expiry);

	public async Task SetCacheAsync(string controllerName, string actionName, object argument, object? value, TimeSpan? expiry = null) {
		await SetCacheByKeyAsync(CreateKey(controllerName, actionName, argument), value, expiry);
	}
	public async Task SetCacheAsync(ActionDto action, object argument, object? value, TimeSpan? expiry = null) =>
		await SetCacheAsync(action.ControllerName, action.ActionName, argument, value, expiry);


	public async Task DeleteCacheByControllerAsync(string controllerName) {
		await DeleteKeysByPatternAsync($"{controllerName}*");
	}
	public async Task DeleteCacheByControllerAsync(ControllerDto controller) =>
		await DeleteCacheByControllerAsync(controller.ControllerName);


	public async Task DeleteCacheByActionAsync(string controllerName, object actionName) {
		await DeleteKeysByPatternAsync($"{controllerName}:{actionName}*");
	}
	public async Task DeleteCacheByActionAsync(ActionDto action) =>
		await DeleteCacheByActionAsync(action.ControllerName, action.ActionName);


	public async Task DeleteCacheByArgumentAsync(string controllerName, string actionName, object argument) {
		await DeleteKeysByPatternAsync($"{controllerName}:{actionName}:{argument}");
	}
	public async Task DeleteCacheByArgumentAsync(ActionDto action, object argument) =>
		await DeleteCacheByArgumentAsync(action.ControllerName, action.ActionName, argument);

	public async Task ClearCacheAsync() {
		foreach (var endpoint in connectionMultiplexer.GetEndPoints()) {
			var server = connectionMultiplexer.GetServer(endpoint);

			await server.FlushAllDatabasesAsync();
		}
	}



	private async Task<bool> IsContainsCacheByKeyAsync(string key) {
		return await _redis.KeyExistsAsync(key);
	}

	private async Task<T> GetCacheByKeyAsync<T>(string key) {
		RedisValue json = await _redis.StringGetAsync(key);

		if (json.IsNullOrEmpty)
			throw new KeyIsNotExistsException("The key is not exists");

		return JsonSerializer.Deserialize<T>(json.ToString())!;
	}

	private async Task<T?> TryGetCacheByKeyAsync<T>(string key) {
		RedisValue json = await _redis.StringGetAsync(key);

		if (json.IsNullOrEmpty)
			return default;

		return JsonSerializer.Deserialize<T>(json.ToString())!;
	}

	private async Task SetCacheByKeyAsync(string key, object? value, TimeSpan? expiry) {
		var json = JsonSerializer.Serialize(value);

		await _redis.StringSetAsync(key, json);

		if (expiry is not null)
			await _redis.KeyExpireAsync(key, expiry);
	}

	public async Task DeleteKeysByPatternAsync(string pattern) {
		await foreach (var key in GetKeysByPatternAsync(pattern)) {
			await _redis.KeyDeleteAsync(key);
		}
	}


	private static string CreateKey(string controllerName, string actionName) =>
		$"{controllerName}:{actionName}";

	private static string CreateKey(ActionDto action) =>
		CreateKey(action.ControllerName, action.ActionName);

	private static string CreateKey(string controllerName, string actionName, object argument) =>
		$"{CreateKey(controllerName, actionName)}:{JsonSerializer.Serialize(argument)}";

	private static string CreateKey(ActionDto action, object argument) =>
		CreateKey(action.ControllerName, action.ActionName, argument);


	public async IAsyncEnumerable<string> GetKeysByPatternAsync(string pattern) {
		if (string.IsNullOrWhiteSpace(pattern))
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(pattern));

		foreach (var endpoint in connectionMultiplexer.GetEndPoints()) {
			var server = connectionMultiplexer.GetServer(endpoint);

			await foreach (var key in server.KeysAsync(pattern: pattern)) {
				yield return key.ToString();
			}
		}
	}
}
