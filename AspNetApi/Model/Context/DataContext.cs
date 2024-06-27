using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.EntityTypeConfigurations;

namespace Model.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options) {
	public DbSet<Category> Categories { get; set; }
	public DbSet<Pizza> Pizzas { get; set; }
	public DbSet<PizzaImage> PizzaImages { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
		new PizzaEntityTypeConfiguration().Configure(modelBuilder.Entity<Pizza>());
		new PizzaImageEntityTypeConfiguration().Configure(modelBuilder.Entity<PizzaImage>());
	}
}