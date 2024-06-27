using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class PizzaIngredientEntityTypeConfiguration : IEntityTypeConfiguration<PizzaIngredient> {
	public void Configure(EntityTypeBuilder<PizzaIngredient> builder) {
		builder.ToTable("PizzaIngredients");

		builder.HasKey(pi => new {
			pi.PizzaId,
			pi.IngredientId
		});
	}
}
