using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phone");
        
        builder.HasKey(phone => phone.Id);

        builder.Property(phone => phone.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(phone => phone.PhoneNumber).HasColumnName("phone_number").HasMaxLength(13).IsRequired();
        builder.Property(phone => phone.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(phone => phone.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(phone => phone.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasOne(entity => entity.User)
           .WithMany()
           .HasForeignKey(entity => entity.UserId);
    }
}