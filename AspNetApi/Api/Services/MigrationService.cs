using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.Context;

namespace Api.Services;

public class MigrationService(
	DataContext context
) : IMigrationService {

	public async Task MigrateLatestAsync() {
		await context.Database.MigrateAsync();
	}

	private async Task<IEnumerable<string>> GetPendingMigrationsAsync()
		=> await context.Database.GetPendingMigrationsAsync();

	private async Task<bool> IsPendingMigrationBeforeOrEqualsAsync(string name) {
		var migrations = await context.Database.GetPendingMigrationsAsync();

		return migrations.TakeWhile(pm => pm != name)
			.Contains(name);
	}

	private async Task<bool> IsPendingMigrationAfterOrEqualsAsync(string name) {
		var migrations = await context.Database.GetPendingMigrationsAsync();

		return migrations.SkipWhile(pm => pm != name)
			.Contains(name);
	}

	private async Task<bool> IsMigrationPendingAsync(string name) {
		var migrations = await context.Database.GetPendingMigrationsAsync();

		return migrations.Contains(name);
	}

	private async Task<bool> IsMigrationNotPendingAsync(string name)
		=> !await IsMigrationPendingAsync(name);
}