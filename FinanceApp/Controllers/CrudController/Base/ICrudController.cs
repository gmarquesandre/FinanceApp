using FinanceApp.Shared.Dto.Base;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController.Base
{
    public interface ICrudController<TCreate, TUpdate>
        where TCreate : CreateDto
        where TUpdate : UpdateDto
    {
        Task<IActionResult> AddAsync(TCreate input);
        Task<IActionResult> DeleteAsync(int id);
        Task<IActionResult> DeleteAllAsync();
        IActionResult DeleteBatch(List<int> ids);
        Task<IActionResult> GetAsync();
        Task<IActionResult> GetAsync(int id);
        Task<IActionResult> UpdateAsync(TUpdate input);
    }
}