using FinanceApp.Shared.Dto.Base;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController.Base
{
    public interface ICrudSingleController<TCreateOrUpdate>
        where TCreateOrUpdate : CreateOrUpdateDto
    {
        Task<IActionResult> CreateOrUpdateAsync(TCreateOrUpdate input);
        Task<IActionResult> DeleteAsync();
        Task<IActionResult> GetAsync();
    }
}