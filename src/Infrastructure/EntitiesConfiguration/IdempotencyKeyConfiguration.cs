using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public sealed class IdempotencyKeyConfiguration : IEntityTypeConfiguration<IdempotencyKey>
{
    public void Configure(EntityTypeBuilder<IdempotencyKey> builder)
    {
        builder.ToTable("idempotency_keys");

        builder.HasKey(record => record.Id);

        builder.Property(record => record.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(record => record.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(record => record.Key).HasColumnName("idempotency_key").HasMaxLength(255).IsRequired();
        builder.Property(record => record.Endpoint).HasColumnName("endpoint").HasMaxLength(500).IsRequired();
        builder.Property(record => record.RequestHash).HasColumnName("request_hash").HasMaxLength(64).IsRequired();
        builder.Property(record => record.ResponseStatus).HasColumnName("response_status").IsRequired();
        builder.Property(record => record.ResponseBody).HasColumnName("response_body").HasColumnType("jsonb").IsRequired(false);
        builder.Property(record => record.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(record => record.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
        builder.Property(record => record.ExpiresAt).HasColumnName("expires_at").IsRequired();

        builder.HasIndex(record => new { record.UserId, record.Endpoint, record.Key }).IsUnique();
        builder.HasIndex(record => record.Key).HasDatabaseName("idx_idempotency_keys_key");
        builder.HasIndex(record => record.ExpiresAt).HasDatabaseName("idx_idempotency_keys_expires");

        builder.ToTable(tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("chk_idempotency_keys_expires", "expires_at > created_at");
        });
    }
}
