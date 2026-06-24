using System;
using System.Collections.Generic;
using System.Text;
using FinancialSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace FinancialSolution.Infrastructure.Configurations
{
    public class ScheduledTransferConfiguration : IEntityTypeConfiguration<ScheduledTransfer>
    {

        public void Configure(EntityTypeBuilder<ScheduledTransfer> builder)
        {

            builder.HasOne(x => x.Customer)
            .WithMany(x => x.ScheduledTransfers)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

            // 2. The Monetary Guarantee
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);
        }
    }
}

