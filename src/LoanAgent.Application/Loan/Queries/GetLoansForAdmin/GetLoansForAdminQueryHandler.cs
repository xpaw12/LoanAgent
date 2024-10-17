using LoanAgent.Application.Common.Dtos;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Queries.GetLoansForAdmin;

public class GetLoansForAdminQueryHandler : IRequestHandler<GetLoansForAdminQuery, List<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoansForAdminQueryHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<LoanDto>> Handle(GetLoansForAdminQuery request, CancellationToken cancellationToken)
    {
        var loans = await _unitOfWork.LoanRepository.FindAllWithUsersAsync(l => l.LoanState != LoanState.InEditMode, cancellationToken);

        return loans.Select(loan => new LoanDto
        {
            LoanId = loan.Id,
            LoanOwnerName = loan.User.FirstName + " " + loan.User.LastName,
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
