using LoanAgent.Application.Common.Dtos;
using LoanAgent.Application.Common.Exceptions;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Application.Common.Services;
using LoanAgent.Domain.Entities;
using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.ChangeLoanStatus;

public class ChangeLoanStatusCommandHandler : IRequestHandler<ChangeLoanStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly LoanPublisher _loanPublisher;

    public ChangeLoanStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        LoanPublisher loanPublisher)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _loanPublisher = loanPublisher;
    }

    public async Task<Unit> Handle(ChangeLoanStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FindAsync(x => x.Id == _currentUser.Id, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(_currentUser.Id, nameof(UserEntity));
        }

        var loan = await _unitOfWork.LoanRepository.FindAsync(l => l.Id == request.LoanId, cancellationToken);

        if (loan is null)
        {
            throw new NotFoundException(request.LoanId, nameof(LoanEntity));
        }

        loan.LoanState = request.NewLoanState;
        loan.UpdatedById = user.Id;
        loan.UpdatedDateTime = DateTime.UtcNow;

        if (user.UserRole == UserRole.User)
        {
            if (request.NewLoanState != LoanState.Submitted)
            {
                throw new UnauthorizedAccessException("Regular users can only change the loan status to 'Submitted'.");
            }
            else
            {
                var loanDto = new LoanDto
                {
                    LoanId = loan.Id,
                    LoanOwnerName = user.FirstName + " " + user.LastName,
                    LoanAmount = loan.LoanAmount,
                    Currency = loan.Currency.ToString(),
                    StartDate = loan.StartDate,
                    EndDate = loan.EndDate,
                    LoanType = loan.LoanType.ToString(),
                    LoanState = loan.LoanState.ToString(),
                    CreatedDateTime = loan.CreatedDateTime,
                    CreatedById = loan.CreatedById,
                    UpdatedDateTime = loan.UpdatedDateTime,
                    UpdatedById = loan.UpdatedById
                };

                _loanPublisher.PublishLoan(loanDto);
            }
        }

        _unitOfWork.LoanRepository.Update(loan);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
