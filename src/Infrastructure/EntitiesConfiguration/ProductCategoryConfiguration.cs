using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(productCategory => productCategory.Id);
        
        builder.ToTable("product_category");

        builder.Property(productCategory => productCategory.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(productCategory => productCategory.Category).HasColumnName("category").HasMaxLength(50).IsRequired();
        builder.Property(productCategory => productCategory.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(productCategory => productCategory.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
    }
}