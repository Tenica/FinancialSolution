using FinancialSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialSolution.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<Wallet> Wallets => Set<Wallet>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<Currency> Currencies => Set<Currency>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<DeviceLog> DeviceLogs => Set<DeviceLog>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    public DbSet<ScheduledTransfer> ScheduledTransfers { get; set; }

    public DbSet<Beneficiary> Beneficiaries { get; set; } = default!;

    public DbSet<TransactionReversalRequest> TransactionReversalRequests { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //ETF automatically find your comfiguration everytime i run a db migration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

     
    }
}

//Expression-Bodied Property public DbSet<Transaction> Transactions => Set<Transaction>();

//Auto property public DbSet<Transaction> Transactions { get; set; }

//db set represents a table/query gateway to the database. It allows you to perform CRUD operations on the corresponding table in the database. When you call Set<Transaction>(), it returns a DbSet<Transaction> that you can use to query and manipulate the Transaction entities in the database.

//Application DbContext //This is the heart of your database access. It is the bridge between your C# code and the SQL Server database. It manages the connection, tracks changes, and translates your C# commands into SQL queries. It also holds DbSet properties for each of your entities, which represent the tables in your database. it translates linq, entities, relationships

//Entity Framework Core Entity Framework Core + Dependency Injection to configure SQL Server connection automatically.
//The OnModelCreating method is where you can configure the model using the Fluent API. By calling ApplyConfigurationsFromAssembly, you are telling Entity Framework to look for any classes that implement IEntityTypeConfiguration<T> in the same assembly as ApplicationDbContext and apply their configurations automatically. This keeps your entity configurations organized and separate from your DbContext.