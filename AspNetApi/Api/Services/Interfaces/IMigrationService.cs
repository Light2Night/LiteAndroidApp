namespace Api.Services.Interfaces;

public interface IMigrationService {
	Task MigrateLatestAsync();
}
