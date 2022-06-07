using AutoMapper;
using FinanceApp.Shared.Models.CommonTables;

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