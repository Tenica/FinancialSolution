using FinancialSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Configurations
{
    public class TransactionReversalRequestConfiguration : IEntityTypeConfiguration<TransactionReversalRequest>
    {
        public void Configure(EntityTypeBuilder<TransactionReversalRequest> builder)
        {
            builder.HasOne(x => x.Customer)
            .WithMany(x => x.ReversalRequests)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.TransactionReference)
                .HasMaxLength(100);

            builder.Property(x => x.Reason)
                .HasMaxLength(500);
        }
    }
}
