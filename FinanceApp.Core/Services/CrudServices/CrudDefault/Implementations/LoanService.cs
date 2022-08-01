using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Core.Services.CrudServices.Base;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class LoanService : CrudServiceBase<Loan, LoanDto, CreateLoan, UpdateLoan>, ILoanService
    {
        public LoanService(IRepository<Loan> repository, IMapper mapper): base(repository, mapper) { }
                                
    }
}