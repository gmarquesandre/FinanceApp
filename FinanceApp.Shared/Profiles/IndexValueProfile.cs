using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Shared.Profiles
{
    public class IndexValueProfile : Profile
    {
        public IndexValueProfile()
        {
            CreateMap<IndexValue, IndexValueDto>();
            CreateMap<IndexValueDto, IndexValue>();
        }
    }
}