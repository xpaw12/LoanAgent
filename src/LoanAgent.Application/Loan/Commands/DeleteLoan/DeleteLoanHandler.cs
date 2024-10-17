using LoanAgent.Application.Common.Exceptions;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Domain.Entities;
using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.DeleteLoan
{
    public class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public DeleteLoanCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
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

            if (loan.UserId != user.Id && user.UserRole != UserRole.Admin)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this loan.");
            }

            _unitOfWork.LoanRepository.SoftDelete(loan);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
