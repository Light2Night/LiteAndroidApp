using Api.Services;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Model.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assemblyName = AssemblyService.GetAssemblyName();

builder.Services.AddDbContext<DataContext>(
	options => {
		options.UseNpgsql(
			builder.Configuration.GetConnectionString("Npgsql"),
			npgsqlOptions => npgsqlOptions.MigrationsAssembly(assemblyName)
		);

		if (builder.Environment.IsDevelopment()) {
			options.EnableSensitiveDataLogging();
		}
	}
);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IImageService, ImageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

string imagesDirPath = app.Services.GetRequiredService<IImageService>().ImagesDir;

if (!Directory.Exists(imagesDirPath)) {
	Directory.CreateDirectory(imagesDirPath);
}

app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(imagesDirPath),
	RequestPath = "/images"
});

app.UseCors(
	configuration => configuration
		.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod()
);

app.UseAuthorization();

app.MapControllers();

await using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope()) {
	await scope.ServiceProvider.GetRequiredService<IMigrationService>().MigrateLatestAsync();
}

app.Run();
