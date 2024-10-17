using LoanAgent.Domain.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanAgent.Infrastructure.Data.Configurations.Common;

internal abstract class AuditableEntityConfig<TEntity>
    : EntityBaseConfig<TEntity>
    where TEntity : AuditableEntity
{
    public override void Configure(
        EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder
            .Property(x => x.CreatedById)
            .IsRequired();

        builder
            .Property(x => x.CreatedDateTime)
            .HasDefaultValueSql("current_timestamp AT TIME ZONE 'UTC'")
            .ValueGeneratedOnAdd();
    }
}