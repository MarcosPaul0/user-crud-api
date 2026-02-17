using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");
        
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Id).HasColumnName("id").ValueGeneratedOnAdd();
        
        builder.Property(product => product.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(product => product.Description).HasColumnName("description").HasMaxLength(1200).IsRequired();
        builder.Property(product => product.PriceInCents).HasColumnName("price_in_cents").IsRequired();
        builder.Property(product => product.DiscountPercentage).HasColumnName("discount_percentage").IsRequired();
        builder.Property(product => product.ProductionTimeInMinutes).HasColumnName("production_time_in_minutes").IsRequired();
        builder.Property(product => product.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(product => product.StockQuantity).HasColumnName("stock_quantity").IsRequired();
        builder.Property(product => product.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(product => product.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
        
        builder.Property(product => product.ProductCategoryId).HasColumnName("product_category_id").IsRequired();
        builder.HasOne(product => product.ProductCategory)
            .WithMany()
            .HasForeignKey(product => product.ProductCategoryId);
    }
}