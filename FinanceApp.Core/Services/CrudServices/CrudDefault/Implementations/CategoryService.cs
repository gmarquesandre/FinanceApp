using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Core.Services.CrudServices.Base;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class CategoryService : CrudServiceBase<Category, CategoryDto, CreateCategory, UpdateCategory>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository, IMapper mapper) : base(repository, mapper) {}      
    }
}