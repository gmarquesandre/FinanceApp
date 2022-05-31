using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ILoanService
    {
        Task<LoanDto> AddAsync(CreateLoan input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<LoanDto>> GetAsync(CustomIdentityUser user);
        Task<LoanDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateLoan input, CustomIdentityUser user);
    }
}