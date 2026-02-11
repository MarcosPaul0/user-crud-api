using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(productImage => productImage.Id);
        
        builder.ToTable("product_image", table =>
        {
            table.HasCheckConstraint(
                "CK_ProductImage_DisplayOrder",
                "display_order BETWEEN 1 AND 5"
            );
        });

        builder.Property(productImage => productImage.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(productImage => productImage.ImageUrl).HasColumnName("image_url").HasMaxLength(500).IsRequired();
        builder.Property(productImage => productImage.DisplayOrder).HasColumnName("display_order").IsRequired();
        builder.Property(productImage => productImage.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(productImage => productImage.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
        
        builder.Property(productImage => productImage.ProductId).HasColumnName("product_id").IsRequired();
        builder.HasOne(productImage => productImage.Product)
            .WithMany()
            .HasForeignKey(productImage => productImage.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(productImage => new { productImage.ProductId, Order = productImage.DisplayOrder }).IsUnique();
    }
}