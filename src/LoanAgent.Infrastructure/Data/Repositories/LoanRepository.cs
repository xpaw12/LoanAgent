using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Domain.Entities;
using LoanAgent.Infrastructure.Data.Repositories.Base;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoanAgent.Infrastructure.Data.Repositories;

public class LoanRepository
: Repository<LoanEntity>, ILoanRepository
{
    public LoanRepository(
        AppDbContext context)
        : base(context)
    {
    }

    public async Task<ICollection<LoanEntity>> FindAllWithUsersAsync(
        Expression<Func<LoanEntity, bool>> match,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(l => l.User)
            .Where(match)
            .ToListAsync(cancellationToken);
    }
}
