using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class PizzaEntityTypeConfiguration : IEntityTypeConfiguration<Pizza> {
	public void Configure(EntityTypeBuilder<Pizza> builder) {
		builder.ToTable("Pizzas");

		builder.Property(p => p.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(p => p.Description)
			.HasMaxLength(4000)
			.IsRequired();
	}
}
