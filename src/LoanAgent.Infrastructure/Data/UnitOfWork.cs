using LoanAgent.Application.Common.Interfaces.Repositories;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Infrastructure.Data.Repositories;

namespace LoanAgent.Infrastructure.Data;

internal class UnitOfWork : IUnitOfWork
{
    private IUserRepository _userRepository;
    private ILoanRepository _loanRepository;

    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_dbContext);

    public ILoanRepository LoanRepository =>
        _loanRepository ??= new LoanRepository(_dbContext);

    public void SaveChanges() =>
        _dbContext.SaveChanges();

    public Task SaveChangesAsync(
        CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
