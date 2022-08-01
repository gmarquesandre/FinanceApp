using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Loan;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ILoanService : IForecast<LoanDto>
        
    {
    }
}