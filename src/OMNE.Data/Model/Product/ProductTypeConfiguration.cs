using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OMNE.Data.Model;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProductEntity> builder) { }
}
