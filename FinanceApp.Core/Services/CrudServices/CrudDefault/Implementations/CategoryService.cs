using AutoMapper;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class CategoryService : CrudBase<Category, CategoryDto, CreateCategory, UpdateCategory>, ICategoryService
    {
        public CategoryService(IRepository<Category> repository, IMapper mapper) : base(repository, mapper) { }
    }
}