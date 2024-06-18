using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Entities;

namespace Model.EntityTypeConfigurations;

internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category> {
	public void Configure(EntityTypeBuilder<Category> builder) {
		builder.ToTable("Categories");

		builder.Property(c => c.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.Property(c => c.Image)
			.HasMaxLength(255)
			.IsRequired();
	}
}
