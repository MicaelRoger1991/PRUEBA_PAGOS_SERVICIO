using EsApp.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsApp.Persistence.Context;

public class EsAppDbContext : DbContext
{
    public EsAppDbContext(DbContextOptions<EsAppDbContext> options) : base(options)
    {
    }

    public DbSet<UsersMasterEntity> UsersMaster => Set<UsersMasterEntity>();
    public DbSet<UsersSessionsMasterEntity> UsersSessionsMaster => Set<UsersSessionsMasterEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<UsersMasterEntity>(entity =>
        {
            entity.ToTable("usersMaster");
            entity.HasKey(e => e.UsersMasterId);

            entity.Property(e => e.UsersMasterId).HasColumnName("usersMasterId");
            entity.Property(e => e.FirstName).HasColumnName("firstName").HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("lastName").HasMaxLength(100);
            entity.Property(e => e.UserRegistration).HasColumnName("userRegistration").HasMaxLength(50);
            entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(50);
            entity.Property(e => e.Role).HasColumnName("role").HasMaxLength(100);
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1);
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");
        });

        modelBuilder.Entity<UsersSessionsMasterEntity>(entity =>
        {
            entity.ToTable("usersSessionsMaster");
            entity.HasKey(e => e.UsersSessionsMasterId);

            entity.Property(e => e.UsersSessionsMasterId).HasColumnName("usersSessionsMasterId");
            entity.Property(e => e.UsersMasterId).HasColumnName("usersMasterId");
            entity.Property(e => e.Token).HasColumnName("token").HasMaxLength(500);
            entity.Property(e => e.RefreshToken).HasColumnName("refreshToken").HasMaxLength(500);
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(100);
            entity.Property(e => e.DateGenerate).HasColumnName("dateGenerate");
            entity.Property(e => e.DateExpiration).HasColumnName("dateExpiration");
            entity.Property(e => e.AmountToken).HasColumnName("amountToken");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");

            entity.HasOne(e => e.UsersMaster)
                .WithMany(u => u.Sessions)
                .HasForeignKey(e => e.UsersMasterId);
        });

        modelBuilder.Entity<UsersMasterEntity>().HasData(
            new UsersMasterEntity
            {
                UsersMasterId = Guid.Parse("0c7c13f1-05cd-4d5a-9ad1-8d2b8f2e1b51"),
                FirstName = "Usuario",
                LastName = "Caja",
                UserRegistration = "caja",
                Password = "123abc",
                Role = "CAJA",
                StateRecord = "A",
                CreationDate = seedDate
            },
            new UsersMasterEntity
            {
                UsersMasterId = Guid.Parse("9c4932e4-1c6d-4b5f-8d7e-7f0c4b13a9b2"),
                FirstName = "Usuario",
                LastName = "Plataforma",
                UserRegistration = "plataforma",
                Password = "123abc",
                Role = "PLATAFORMA",
                StateRecord = "A",
                CreationDate = seedDate
            },
            new UsersMasterEntity
            {
                UsersMasterId = Guid.Parse("a7d3a1f4-0b2c-4f91-8a86-4db5e7c7d4e6"),
                FirstName = "Usuario",
                LastName = "Gerente",
                UserRegistration = "gerente",
                Password = "123abc",
                Role = "GERENTE",
                StateRecord = "A",
                CreationDate = seedDate
            }
        );
    }
}
