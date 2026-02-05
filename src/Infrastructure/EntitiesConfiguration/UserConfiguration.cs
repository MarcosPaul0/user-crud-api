using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Enums;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");
        
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(user => user.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(user => user.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(user => user.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
        builder.Property(user => user.Role).HasColumnName("role").HasDefaultValue(UserRole.Customer).IsRequired();
        builder.Property(user => user.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd().IsRequired();
        builder.Property(user => user.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
    }
}