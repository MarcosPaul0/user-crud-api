using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.Context
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Phone> Phone { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Aplica as configurações das entidades definidas que estendem de IEntityTypeConfiguration<>
      builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
  }
}

