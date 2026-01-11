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
    public DbSet<CustomersEntity> Customers => Set<CustomersEntity>();
    public DbSet<CurrencyEntity> Currency => Set<CurrencyEntity>();
    public DbSet<ServiceProviderEntity> ServiceProvider => Set<ServiceProviderEntity>();
    public DbSet<PaymentsServicesEntity> PaymentsServices => Set<PaymentsServicesEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var bobCurrencyId = Guid.Parse("d29fba7e-1a2b-4fe7-8c2f-813a4e5ec5a0");
        var usdCurrencyId = Guid.Parse("5d8f6a1d-9c2a-4b2f-9d2c-3189d9a21c4b");

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
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1).HasDefaultValue("A");
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

        modelBuilder.Entity<CustomersEntity>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.FirstName).HasColumnName("firstName").HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("lastName").HasMaxLength(100);
            entity.Property(e => e.DocumentNumber).HasColumnName("documentNumber").HasMaxLength(15);
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1).HasDefaultValue("A");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");
        });

        modelBuilder.Entity<CurrencyEntity>(entity =>
        {
            entity.ToTable("currency");
            entity.HasKey(e => e.CurrencyId);

            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.Currency).HasColumnName("currency").HasMaxLength(100);
            entity.Property(e => e.ShortName).HasColumnName("shortName").HasMaxLength(10);
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1).HasDefaultValue("A");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");
        });

        modelBuilder.Entity<ServiceProviderEntity>(entity =>
        {
            entity.ToTable("serviceProvider");
            entity.HasKey(e => e.ServiceProviderId);

            entity.Property(e => e.ServiceProviderId).HasColumnName("serviceProviderId");
            entity.Property(e => e.Service).HasColumnName("service").HasMaxLength(150);
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1).HasDefaultValue("A");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");

            entity.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId);
        });

        modelBuilder.Entity<PaymentsServicesEntity>(entity =>
        {
            entity.ToTable("paymentsServices");
            entity.HasKey(e => e.PaymentsId);

            entity.Property(e => e.PaymentsId).HasColumnName("paymentsId");
            entity.Property(e => e.UsersMasterId).HasColumnName("usersMasterId");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.ServiceProviderId).HasColumnName("serviceProviderId");
            entity.Property(e => e.Amount).HasColumnName("amount").HasPrecision(10, 2);
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(100);
            entity.Property(e => e.StateRecord).HasColumnName("stateRecord").HasMaxLength(1).HasDefaultValue("A");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate").HasDefaultValueSql("now()");
            entity.Property(e => e.ModificationDate).HasColumnName("modificationDate");

            entity.HasOne(e => e.UsersMaster)
                .WithMany(u => u.Payments)
                .HasForeignKey(e => e.UsersMasterId);

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(e => e.CustomerId);

            entity.HasOne(e => e.ServiceProvider)
                .WithMany(s => s.Payments)
                .HasForeignKey(e => e.ServiceProviderId);
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

        modelBuilder.Entity<CustomersEntity>().HasData(
            new CustomersEntity
            {
                CustomerId = Guid.Parse("81b7d3a3-9e4b-4f5c-9d64-3d1d8f9b1a6c"),
                FirstName = "Carlos",
                LastName = "Lopez",
                DocumentNumber = "12345678",
                StateRecord = "A",
                CreationDate = seedDate
            },
            new CustomersEntity
            {
                CustomerId = Guid.Parse("f3dbb0f0-9c42-4fa1-8e4d-1437254a9d0f"),
                FirstName = "Maria",
                LastName = "Fernandez",
                DocumentNumber = "87654321",
                StateRecord = "A",
                CreationDate = seedDate
            }
        );

        modelBuilder.Entity<CurrencyEntity>().HasData(
            new CurrencyEntity
            {
                CurrencyId = bobCurrencyId,
                Currency = "Bolivianos",
                ShortName = "BOB",
                StateRecord = "A",
                CreationDate = seedDate
            },
            new CurrencyEntity
            {
                CurrencyId = usdCurrencyId,
                Currency = "Dolares",
                ShortName = "USD",
                StateRecord = "A",
                CreationDate = seedDate
            }
        );

        modelBuilder.Entity<ServiceProviderEntity>().HasData(
            new ServiceProviderEntity
            {
                ServiceProviderId = Guid.Parse("b6b2d52c-1b8f-4ec8-9b38-0de5f5cc6e92"),
                Service = "Agua",
                CurrencyId = bobCurrencyId,
                StateRecord = "A",
                CreationDate = seedDate
            },
            new ServiceProviderEntity
            {
                ServiceProviderId = Guid.Parse("6e3dce1a-148a-4bf8-9b08-4b4a1e4cbe5a"),
                Service = "Electricidad",
                CurrencyId = bobCurrencyId,
                StateRecord = "A",
                CreationDate = seedDate
            },
            new ServiceProviderEntity
            {
                ServiceProviderId = Guid.Parse("10c5e8f7-7c58-4d8e-9c4d-8c4d5d7a9d1f"),
                Service = "Telecomunicaciones",
                CurrencyId = bobCurrencyId,
                StateRecord = "A",
                CreationDate = seedDate
            }
        );
    }
}
