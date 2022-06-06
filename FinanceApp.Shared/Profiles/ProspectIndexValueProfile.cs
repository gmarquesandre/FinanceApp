using AutoMapper;
using FinanceApp.Shared.Dto;

namespace FinanceApp.Shared.Profiles
{
    public class ProspectIndexValue : Profile
    {
        public ProspectIndexValue()
        {
            CreateMap<ProspectIndexValue, ProspectIndexValueDto>();
            CreateMap<ProspectIndexValueDto, ProspectIndexValue>();
        }
    }
}