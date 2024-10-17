using LoanAgent.Application.Common.Dtos;
using LoanAgent.Application.Common.Interfaces.Helpers;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;

using MediatR;

namespace LoanAgent.Application.Loan.Queries.GetLoansByUserId;

public class GetLoansByUserIdQueryHandler : IRequestHandler<GetLoansByUserIdQuery, List<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public readonly ICurrentUser _currentUser;

    public GetLoansByUserIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }


    public async Task<List<LoanDto>> Handle(GetLoansByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _currentUser.Id;

        var loans = await _unitOfWork.LoanRepository.FindAllAsync(l => l.UserId == userId, cancellationToken);

        return loans.Select(loan => new LoanDto
        {
            LoanId = loan.Id,
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
        }).ToList();
    }
}
