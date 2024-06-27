using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class IngredientEntityTypeConfiguration : IEntityTypeConfiguration<Ingredient> {
	public void Configure(EntityTypeBuilder<Ingredient> builder) {
		builder.ToTable("Ingredients");

		builder.Property(i => i.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(i => i.Image)
			.HasMaxLength(255)
			.IsRequired();
	}
}
