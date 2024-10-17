using LoanAgent.Application.Common.Interfaces.Repositories.Base;
using LoanAgent.Domain.Entities;

namespace LoanAgent.Application.Common.Interfaces.Repositories;

public interface ILoanRepository : IRepository<LoanEntity>
{
}
