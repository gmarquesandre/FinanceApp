using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Category;
using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.Api.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategory, Category>();
            CreateMap<UpdateCategory, Category>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<UpdateCategory, Category>();
        }
    }
}