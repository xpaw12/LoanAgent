using LoanAgent.Domain.Common;

using System.Linq.Expressions;

namespace LoanAgent.Application.Common.Interfaces.Repositories.Base;

public interface IRepository<TEntity>
    where TEntity : EntityBase
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    void Add(TEntity entity);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> match, CancellationToken cancellationToken = default);

    Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match, CancellationToken cancellationToken = default);

    void UpdateProperties(
        TEntity entity,
        params string[] propertyNames);

    void SoftDelete(TEntity entity);

    void SoftDeleteRange(IEnumerable<TEntity> entities);
}
