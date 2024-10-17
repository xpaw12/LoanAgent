using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Domain.Entities;
using LoanAgent.Infrastructure.Data.Repositories.Base;

namespace LoanAgent.Infrastructure.Data.Repositories;

public class LoanRepository
: Repository<LoanEntity>, ILoanRepository
{
    public LoanRepository(
        AppDbContext context)
        : base(context)
    {
    }
}
