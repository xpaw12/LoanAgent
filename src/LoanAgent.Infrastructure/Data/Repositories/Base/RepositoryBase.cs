using LoanAgent.Application.Common.Interfaces.Repositories.Base;
using LoanAgent.Domain.Common;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace LoanAgent.Infrastructure.Data.Repositories.Base;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : EntityBase
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken) =>
        DbSet.ToListAsync(cancellationToken);

    public void Add(TEntity entity) =>
        DbSet.Add(entity);

    public virtual async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public void AddRange(IEnumerable<TEntity> entities) =>
        DbSet.AddRange(entities);

    public void Update(TEntity entity) =>
        DbContext.Entry(entity).State = EntityState.Modified;

    public virtual async Task<TEntity?> FindAsync(
        Expression<Func<TEntity,
        bool>> match,
        CancellationToken cancellationToken = default)
    {
        var matchingEntity = await DbSet
            .FirstOrDefaultAsync(match, cancellationToken);

        return matchingEntity is not null
            ? matchingEntity
            : null;
    }

    public async Task<ICollection<TEntity>> FindAllAsync(
        Expression<Func<TEntity,
        bool>> match,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(match).ToListAsync(cancellationToken);
    }

    public void UpdateProperties(
        TEntity entity,
        params string[] propertyNames)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var entry = DbContext.Entry(entity);

        foreach (var propName in propertyNames)
            entry.Property(propName).IsModified = true;
    }

    public void SoftDelete(TEntity entity)
    {
        if (entity is not EntityBase entityBase)
            throw new InvalidOperationException($"{nameof(entity)} can not be soft deleted");

        var flagProperty = DbContext
            .Entry(entityBase)
            .Property(e => e.Deleted);

        flagProperty.CurrentValue = true;
        flagProperty.IsModified = true;
    }

    public void SoftDeleteRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            SoftDelete(entity);
    }
}
