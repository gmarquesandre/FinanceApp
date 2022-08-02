using FinanceApp.Api.Controllers.CrudController.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto.Category;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.Api.Controllers.CrudController
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : CrudControllerBase<ICategoryService, CategoryDto, CreateCategory, UpdateCategory>
    {

        public CategoryController(ICategoryService service) : base(service)
        {
        }
      
    }
}