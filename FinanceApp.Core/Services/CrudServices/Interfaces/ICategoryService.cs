using FinanceApp.Api.Startup;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ICategoryService : ITransientService
    {
        Task<CategoryDto> AddAsync(CreateCategory input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<CategoryDto>> GetAsync(CustomIdentityUser user);
        Task<CategoryDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateCategory input, CustomIdentityUser user);
    }
}