using LoanAgent.Application.Common.Interfaces.Repositories.Base;
using LoanAgent.Domain.Entities;

using System.Linq.Expressions;

namespace LoanAgent.Application.Common.Interfaces.Repositories;

public interface ILoanRepository : IRepository<LoanEntity>
{
    Task<ICollection<LoanEntity>> FindAllWithUsersAsync(Expression<Func<LoanEntity, bool>> match, CancellationToken cancellationToken = default);
}
