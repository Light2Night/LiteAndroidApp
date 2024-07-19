namespace Api.Extensions;

public static class IWebHostEnvironmentExtensions {
	public static bool IsDocker(this IWebHostEnvironment environment) {
		return environment.EnvironmentName == "Docker";
	}
}
