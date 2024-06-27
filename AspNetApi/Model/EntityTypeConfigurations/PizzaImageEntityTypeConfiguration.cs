using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class PizzaImageEntityTypeConfiguration : IEntityTypeConfiguration<PizzaImage> {
	public void Configure(EntityTypeBuilder<PizzaImage> builder) {
		builder.ToTable("PizzaImages");

		builder.Property(pi => pi.Name)
			.HasMaxLength(255)
			.IsRequired();
	}
}
