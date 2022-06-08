using AutoMapper;
using FinanceApp.Shared.Models.CommonTables;

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