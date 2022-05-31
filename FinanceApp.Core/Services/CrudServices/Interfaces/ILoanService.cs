using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
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