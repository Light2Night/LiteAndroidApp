using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Entities.Identity;
using Model.EntityTypeConfigurations;
using Model.EntityTypeConfigurations.Identity;

namespace Model.Context;

public class DataContext(DbContextOptions<DataContext> options)
	: IdentityDbContext<
		User,
		Role,
		long,
		IdentityUserClaim<long>,
		UserRole,
		IdentityUserLogin<long>,
		IdentityRoleClaim<long>,
		IdentityUserToken<long>
	>(options) {

	public DbSet<Category> Categories { get; set; }
	public DbSet<Pizza> Pizzas { get; set; }
	public DbSet<PizzaImage> PizzaImages { get; set; }
	public DbSet<Ingredient> Ingredients { get; set; }
	public DbSet<PizzaIngredient> PizzaIngredients { get; set; }
	public DbSet<Size> Sizes { get; set; }
	public DbSet<PizzaSize> PizzaSizes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
		new UserRoleEntityTypeConfiguration().Configure(modelBuilder.Entity<UserRole>());

		new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
		new PizzaEntityTypeConfiguration().Configure(modelBuilder.Entity<Pizza>());
		new PizzaImageEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaImage>());
		new IngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<Ingredient>());
		new PizzaIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaIngredient>());
		new SizeEntityTypeConfiguration().Configure(modelBuilder.Entity<Size>());
		new PizzaSizeEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaSize>());
	}
}