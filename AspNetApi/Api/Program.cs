using Api.Configurations;
using Api.Extensions;
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
using Api.ViewModels.SpecificationName;
using Api.ViewModels.SpecificationValue;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.Context;
using Model.Entities.Identity;
using StackExchange.Redis;
using System.Text;

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

builder.Services
	.AddIdentity<User, Model.Entities.Identity.Role>(options => {
		options.Stores.MaxLengthForKeys = 128;

		options.Password.RequiredLength = 8;
		options.Password.RequireDigit = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	})
	.AddEntityFrameworkStores<DataContext>()
	.AddDefaultTokenProviders();

var singinKey = new SymmetricSecurityKey(
	Encoding.UTF8.GetBytes(
		builder.Configuration["Authentication:Jwt:SecretKey"]
			?? throw new NullReferenceException("Authentication:Jwt:SecretKey")
	)
);

builder.Services
	.AddAuthentication(options => {
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options => {
		options.SaveToken = true;
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters() {
			ValidateIssuer = false,
			ValidateAudience = false,
			IssuerSigningKey = singinKey,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ClockSkew = TimeSpan.Zero
		};
	});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
	options.AddSecurityDefinition(
		"Bearer",
		new OpenApiSecurityScheme {
			Description = "Jwt Auth header using the Bearer scheme",
			Type = SecuritySchemeType.Http,
			Scheme = "bearer"
		}
	);
	options.AddSecurityRequirement(new OpenApiSecurityRequirement {
		{
			new OpenApiSecurityScheme {
				Reference = new OpenApiReference {
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			},
			new List<string>()
		}
	});
});


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
builder.Services.AddScoped<IIdentitySeeder, IdentitySeeder>();

builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IImageValidator, ImageValidator>();
builder.Services.AddTransient<IExistingEntityCheckerService, ExistingEntityCheckerService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddTransient<IAccountsControllerService, AccountsControllerService>();

builder.Services.AddTransient<ICategoriesControllerService, CategoriesControllerService>();
builder.Services.AddTransient<IPaginationService<CategoryVm, CategoryFilterVm>, CategoriesPaginationService>();

builder.Services.AddTransient<IIngredientsControllerService, IngredientsControllerService>();
builder.Services.AddTransient<IPaginationService<IngredientVm, IngredientFilterVm>, IngredientsPaginationService>();

builder.Services.AddTransient<IPizzasControllerService, PizzasControllerService>();
builder.Services.AddTransient<IPaginationService<PizzaVm, PizzaFilterVm>, PizzasPaginationService>();

builder.Services.AddTransient<ISizesControllerService, SizesControllerService>();
builder.Services.AddTransient<IPaginationService<SizeVm, SizeFilterVm>, SizesPaginationService>();

builder.Services.AddTransient<IPizzaSizesControllerService, PizzaSizesControllerService>();

builder.Services.AddTransient<ISpecificationValuesControllerService, SpecificationValuesControllerService>();
builder.Services.AddTransient<IPaginationService<SpecificationValueVm, SpecificationValueFilterVm>, SpecificationValuesPaginationService>();

builder.Services.AddTransient<ISpecificationNamesControllerService, SpecificationNamesControllerService>();
builder.Services.AddTransient<IPaginationService<SpecificationNameVm, SpecificationNameFilterVm>, SpecificationNamesPaginationService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsDocker()) {
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
	await scope.ServiceProvider.GetRequiredService<IIdentitySeeder>().SeedAsync();
}

app.Run();
