using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Shared.Profiles
{
    public class ProspectIndexValueProfile : Profile
    {
        public ProspectIndexValueProfile()
        {
            CreateMap<ProspectIndexValue, ProspectIndexValueDto>();
            CreateMap<ProspectIndexValueDto, ProspectIndexValue>();
        }
    }
}