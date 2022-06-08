using AutoMapper;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models.CommonTables;

namespace FinanceApp.Shared.Profiles
{
    public class WorkingDaysByYearProfile  : Profile
    {
        public WorkingDaysByYearProfile()
        {
            CreateMap<WorkingDaysByYear, WorkingDaysByYearDto>();
            CreateMap<WorkingDaysByYearDto, WorkingDaysByYear>();
        }
    }
}