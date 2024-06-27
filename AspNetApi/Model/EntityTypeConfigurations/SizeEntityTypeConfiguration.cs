using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class SizeEntityTypeConfiguration : IEntityTypeConfiguration<Size> {
	public void Configure(EntityTypeBuilder<Size> builder) {
		builder.ToTable("Sizes");

		builder.Property(s => s.Name)
			.HasMaxLength(50)
			.IsRequired();
	}
}
