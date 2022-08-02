using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.Shared.Dto.Category;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface ICategoryService: ICrudBase<CategoryDto, CreateCategory, UpdateCategory>        
    {
    }
}