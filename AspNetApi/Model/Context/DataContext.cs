using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.EntityTypeConfigurations;

namespace Model.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options) {
	public DbSet<Category> Categories { get; set; }
	public DbSet<Pizza> Pizzas { get; set; }
	public DbSet<PizzaImage> PizzaImages { get; set; }
	public DbSet<Ingredient> Ingredients { get; set; }
	public DbSet<PizzaIngredient> PizzaIngredients { get; set; }
	public DbSet<Size> Sizes { get; set; }
	public DbSet<PizzaSize> PizzaSizes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
		new PizzaEntityTypeConfiguration().Configure(modelBuilder.Entity<Pizza>());
		new PizzaImageEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaImage>());
		new IngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<Ingredient>());
		new PizzaIngredientEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaIngredient>());
		new SizeEntityTypeConfiguration().Configure(modelBuilder.Entity<Size>());
		new PizzaSizeEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaSize>());
	}
}