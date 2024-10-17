using LoanAgent.Application.Common.Dtos;
using LoanAgent.Application.Common.Interfaces.UnitOfWork;
using LoanAgent.Domain.Enums;

using MediatR;

namespace LoanAgent.Application.Loan.Queries.GetLoansForAdmin;

public class GetLoansForAdminQueryHandler : IRequestHandler<GetLoansForAdminQuery, List<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly LoanConsumer _loanConsumer;
    //private readonly List<LoanEntity> _loans = new();

    public GetLoansForAdminQueryHandler(
        IUnitOfWork unitOfWork)
        //LoanConsumer loanConsumer)
    {
        _unitOfWork = unitOfWork;
        //_loanConsumer = loanConsumer;

        //_loanConsumer.StartConsuming(ReceiveLoan);
    }

    public async Task<List<LoanDto>> Handle(GetLoansForAdminQuery request, CancellationToken cancellationToken)
    {
        var loans = await _unitOfWork.LoanRepository.FindAllAsync(l => l.LoanState == LoanState.Submitted, cancellationToken);

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

    //private async Task ReceiveLoan(LoanEntity loan)
    //{
    //    _loans.Add(loan);
    //}
}
