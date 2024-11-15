using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.EntitiesConfiguration
{
    public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
    {
        public void Configure(EntityTypeBuilder<Phone> builder)
        {
            builder.HasKey(phone => phone.Id);

            builder.Property(user => user.CreatedAt).ValueGeneratedOnAdd();
            builder.Property(user => user.UpdatedAt).ValueGeneratedOnAdd();
            builder.Property(phone => phone.Id).ValueGeneratedOnAdd();
            builder.Property(phone => phone.PhoneNumber).HasMaxLength(13).IsRequired();

            builder.HasOne(entity => entity.User)
               .WithMany(entity => entity.Phones)
               .HasForeignKey(entity => entity.UserId);
        }
    }
}