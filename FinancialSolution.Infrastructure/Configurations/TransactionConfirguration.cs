using FinancialSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialSolution.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Reference)
            .HasMaxLength(100)
            .IsRequired();

       builder.HasOne(x => x.Wallet)
      .WithMany(x => x.Transactions)
      .HasForeignKey(x => x.WalletId)
      .OnDelete(DeleteBehavior.Restrict);
    }
}