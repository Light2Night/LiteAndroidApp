using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class SpecificationValueEntityTypeConfiguration : IEntityTypeConfiguration<SpecificationValue> {
	public void Configure(EntityTypeBuilder<SpecificationValue> builder) {
		builder.ToTable("SpecificationValues");

		builder.Property(sv => sv.Value)
			.HasMaxLength(50)
			.IsRequired();
	}
}
