using LoanAgent.Domain.Entities;
using LoanAgent.Infrastructure.Data.Configurations.Common;
using LoanAgent.Infrastructure.Data.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanAgent.Infrastructure.Data.Configurations;

internal class LoanEntityConfig
    : AuditableEntityConfig<LoanEntity>
{
    public override void Configure(
        EntityTypeBuilder<LoanEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.LoanAmount)
               .HasColumnType(EntityConfigConstants.DecimalColumnType)
               .IsRequired();

        builder.Property(x => x.Currency)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(x => x.StartDate)
               .HasColumnType(EntityConfigConstants.DateOfBirthColumnType)
               .IsRequired();

        builder.Property(x => x.EndDate)
               .HasColumnType(EntityConfigConstants.DateOfBirthColumnType)
               .IsRequired();

        builder.Property(x => x.LoanType)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(x => x.LoanState)
               .HasConversion<int>()
               .IsRequired();

        builder.HasOne(x => x.User)
               .WithMany(x => x.Loans)
               .HasForeignKey(x => x.UserId)
               .IsRequired();

        builder.ToTable("Loans");
    }
}
