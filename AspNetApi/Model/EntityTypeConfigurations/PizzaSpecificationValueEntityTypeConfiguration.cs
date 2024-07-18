using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class PizzaSpecificationValueEntityTypeConfiguration : IEntityTypeConfiguration<PizzaSpecificationValue> {
	public void Configure(EntityTypeBuilder<PizzaSpecificationValue> builder) {
		builder.ToTable("PizzaSpecificationValues");

		builder.HasKey(psv => new {
			psv.PizzaId,
			psv.SpecificationValueId
		});
	}
}
