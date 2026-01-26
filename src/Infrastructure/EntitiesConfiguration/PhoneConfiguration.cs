using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.HasKey(phone => phone.Id);
        
        builder.ToTable("phone");

        builder.Property(phone => phone.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(phone => phone.PhoneNumber).HasColumnName("phone_number").HasMaxLength(13).IsRequired();
        builder.Property(phone => phone.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(user => user.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(user => user.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasOne(entity => entity.User)
           .WithMany(entity => entity.Phones)
           .HasForeignKey(entity => entity.UserId);
    }
}