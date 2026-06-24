using FinancialSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialSolution.Infrastructure.Configurations;

public class BeneficiaryConfiguration
    : IEntityTypeConfiguration<Beneficiary>
{
    public void Configure(
        EntityTypeBuilder<Beneficiary> builder)
    {
        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Beneficiaries)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.BeneficiaryName)
            .HasMaxLength(200);

        builder.Property(x => x.AccountNumber)
            .HasMaxLength(50);

        builder.Property(x => x.BankName)
            .HasMaxLength(200);
    }
}
