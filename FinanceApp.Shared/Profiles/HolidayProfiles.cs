using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Entities.CommonTables;

namespace FinanceApp.Shared.Profiles
{
    public class HolidayProfile : Profile
    {
        public HolidayProfile()
        {
            CreateMap<Holiday, HolidayDto>();
            CreateMap<HolidayDto, Holiday>();
        }
    }
}