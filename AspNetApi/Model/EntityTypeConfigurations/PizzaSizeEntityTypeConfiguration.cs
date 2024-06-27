using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class PizzaSizeEntityTypeConfiguration : IEntityTypeConfiguration<PizzaSize> {
	public void Configure(EntityTypeBuilder<PizzaSize> builder) {
		builder.ToTable("PizzaSizes");

		builder.HasKey(ps => new {
			ps.PizzaId,
			ps.SizeId
		});
	}
}
