using LoanAgent.Domain.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanAgent.Infrastructure.Data.Configurations.Common;

internal abstract class EntityBaseConfig<TEntity>
: IEntityTypeConfiguration<TEntity>
where TEntity : EntityBase
{
    public virtual void Configure(
        EntityTypeBuilder<TEntity> builder) =>
        builder
            .HasQueryFilter(e => !e.Deleted);
}
