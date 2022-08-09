using AutoMapper;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Entities.UserTables;

namespace FinanceApp.Shared.Profiles
{
    public class FGTSProfile : Profile
    {
        public FGTSProfile()
        {
            CreateMap<CreateOrUpdateFGTS, FGTS>();
            CreateMap<FGTS, CreateOrUpdateFGTS>();
            CreateMap<FGTSDto, FGTS>();
            CreateMap<FGTS, FGTSDto>();
            CreateMap<FGTSDto, FGTSSpread>();
        }
    }
}