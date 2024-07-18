using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class SpecificationNameEntityTypeConfiguration : IEntityTypeConfiguration<SpecificationName> {
	public void Configure(EntityTypeBuilder<SpecificationName> builder) {
		builder.ToTable("SpecificationNames");

		builder.Property(sn => sn.Name)
			.HasMaxLength(50)
			.IsRequired();
	}
}
