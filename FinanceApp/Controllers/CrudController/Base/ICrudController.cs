using FinanceApp.Shared.Dto.Base;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController.Base
{
    public interface ICrudController<TCreate, TUpdate>
        where TCreate : CreateDto
        where TUpdate : UpdateDto
    {
        Task<IActionResult> AddAsync(TCreate input);
        Task<IActionResult> DeleteAsync(Guid id);
        Task<IActionResult> DeleteAllAsync();
        IActionResult DeleteBatch(List<Guid> ids);
        Task<IActionResult> GetAsync();
        Task<IActionResult> GetAsync(Guid id);
        Task<IActionResult> UpdateAsync(TUpdate input);
    }
}