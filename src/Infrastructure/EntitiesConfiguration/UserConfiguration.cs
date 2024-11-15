using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .ValueGeneratedOnAdd();
            builder.Property(user => user.CreatedAt)
                .ValueGeneratedOnAdd();
            builder.Property(user => user.UpdatedAt).ValueGeneratedOnAdd();
            builder.Property(user => user.Name).HasMaxLength(50).IsRequired();
            builder.Property(user => user.Email).HasMaxLength(255).IsRequired();
            builder.Property(user => user.Password).HasMaxLength(50).IsRequired();

            builder.HasMany(entity => entity.Phones)
                .WithOne(entity => entity.User)
                .HasForeignKey(entity => entity.UserId)
                .IsRequired();
        }
    }
}