using FinanceApp.Shared;
using FinanceApp.Shared.Dto.Category;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ICategoryService : IScopedService
    {
        Task<CategoryDto> AddAsync(CreateCategory input);
        Task<Result> DeleteAsync(int id);
        Task<List<CategoryDto>> GetAsync();
        Task<CategoryDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateCategory input);
    }
}