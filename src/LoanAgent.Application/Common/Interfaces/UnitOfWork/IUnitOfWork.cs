using LoanAgent.Application.Common.Interfaces.Repositories;

namespace LoanAgent.Application.Common.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public ILoanRepository LoanRepository { get; }
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken);
    public void Dispose();
}
