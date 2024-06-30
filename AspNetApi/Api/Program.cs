using Api.Configurations;
using Api.Mapper;
using Api.Services;
using Api.Services.ControllerServices;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.Services.PaginationServices;
using Api.ViewModels.Category;
using Api.ViewModels.Ingredient;
using Api.ViewModels.Pizza;
using Api.ViewModels.Size;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Model.Context;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(
	options => {
		options.UseNpgsql(
			builder.Configuration.GetConnectionString("Npgsql"),
			npgsqlOptions => npgsqlOptions.MigrationsAssembly(AssemblyService.GetAssemblyName())
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


builder.Services.Configure<CacheExpirySeconds>(builder.Configuration.GetSection("CacheExpirySeconds"));

builder.Services.AddAutoMapper(typeof(AppMapProfile));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
	ConnectionMultiplexer.Connect(
		builder.Configuration.GetConnectionString("Redis")
			?? throw new NullReferenceException("Redis connection string is not inicialized")
	)
);

builder.Services.AddScoped<IMigrationService, MigrationService>();

builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IImageValidator, ImageValidator>();
builder.Services.AddTransient<IExistingEntityCheckerService, ExistingEntityCheckerService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddTransient<ICategoriesControllerService, CategoriesControllerService>();
builder.Services.AddTransient<IPaginationService<CategoryVm, CategoryFilterVm>, CategoriesPaginationService>();

builder.Services.AddTransient<IIngredientsControllerService, IngredientsControllerService>();
builder.Services.AddTransient<IPaginationService<IngredientVm, IngredientFilterVm>, IngredientsPaginationService>();

builder.Services.AddTransient<IPizzasControllerService, PizzasControllerService>();
builder.Services.AddTransient<IPaginationService<PizzaVm, PizzaFilterVm>, PizzasPaginationService>();

builder.Services.AddTransient<ISizesControllerService, SizesControllerService>();
builder.Services.AddTransient<IPaginationService<SizeVm, SizeFilterVm>, SizesPaginationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker") {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(
	configuration => configuration
		.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod()
);

var imageService = app.Services.GetRequiredService<IImageService>();
imageService.CreateWorkingDirectoryIfNotExists();

app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(imageService.ImagesDir),
	RequestPath = "/images"
});

app.UseAuthorization();

app.MapControllers();

await using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope()) {
	await scope.ServiceProvider.GetRequiredService<IMigrationService>().MigrateLatestAsync();
}

app.Run();
