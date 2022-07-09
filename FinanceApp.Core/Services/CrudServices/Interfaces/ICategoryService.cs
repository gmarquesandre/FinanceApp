using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ICategoryService : ITransientService
    {
        Task<CategoryDto> AddAsync(CreateCategory input);
        Task<Result> DeleteAsync(int id);
        Task<List<CategoryDto>> GetAsync();
        Task<CategoryDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdateCategory input);
    }
}