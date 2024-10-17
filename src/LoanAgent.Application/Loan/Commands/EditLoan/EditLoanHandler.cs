using LoanAgent.Application.Common.Exceptions;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Domain.Entities;

using MediatR;

namespace LoanAgent.Application.Loan.Commands.EditLoan;

public class EditLoanCommandHandler : IRequestHandler<EditLoanCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public EditLoanCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(EditLoanCommand request, CancellationToken cancellationToken)
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

        if (loan.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this loan.");
        }

        var modifiedProperties = UpdateProperties(loan, request, user.Id);

        if (modifiedProperties.Any())
        {
            _unitOfWork.LoanRepository.UpdateProperties(loan, modifiedProperties.ToArray());
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return loan.Id;
    }

    private static List<string> UpdateProperties(LoanEntity loan, EditLoanCommand request, Guid updatedById)
    {
        var modifiedProperties = new List<string>();

        var loanProperties = typeof(LoanEntity).GetProperties();
        var requestProperties = typeof(EditLoanCommand).GetProperties();

        foreach (var prop in requestProperties)
        {
            var loanProp = loanProperties.FirstOrDefault(p => p.Name == prop.Name);
            if (loanProp != null)
            {
                var currentValue = loanProp.GetValue(loan);
                var newValue = prop.GetValue(request);

                if (!Equals(currentValue, newValue))
                {
                    loanProp.SetValue(loan, newValue);
                    modifiedProperties.Add(prop.Name);
                }
            }
        }

        loan.UpdatedById = updatedById;
        loan.UpdatedDateTime = DateTime.UtcNow;

        modifiedProperties.Add(nameof(loan.UpdatedById));
        modifiedProperties.Add(nameof(loan.UpdatedDateTime));

        return modifiedProperties;
    }
}
