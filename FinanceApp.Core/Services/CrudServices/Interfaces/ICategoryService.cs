using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddAsync(CreateCategory input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<CategoryDto>> GetAsync(CustomIdentityUser user);
        Task<CategoryDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateCategory input, CustomIdentityUser user);
    }
}