using LoanAgent.Application.Common.Exceptions;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Domain.Entities;
using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.CreateLoan;

public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateLoanCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(x => x.Id == _currentUser.Id, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(_currentUser.Id, nameof(UserEntity));
        }

        var newLoan = new LoanEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            LoanAmount = request.LoanAmount,
            Currency = request.Currency,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            LoanType = request.LoanType,
            LoanState = LoanState.InEditMode,
            CreatedDateTime = DateTime.UtcNow,
            CreatedById = user.Id
        };

        _unitOfWork.LoanRepository.Add(newLoan);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newLoan.Id;
    }
}