using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface IFGTSService
    {
        Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input, CustomIdentityUser user);
        Task<Result> DeleteAsync(CustomIdentityUser user);
        Task<FGTSDto> GetAsync(CustomIdentityUser user);
    }
}