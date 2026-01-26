using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.ToTable("user");

        builder.Property(user => user.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(user => user.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(user => user.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(user => user.Password).HasColumnName("password").HasMaxLength(50).IsRequired();
        builder.Property(user => user.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd();
        builder.Property(user => user.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasMany(entity => entity.Phones)
            .WithOne(entity => entity.User)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired();
    }
}