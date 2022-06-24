using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services
{
    public interface IDatesService : IScopedService
    {
        Task<WorkingDaysByYearDto> GetWorkingDaysOfAYear(int year);
        Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear(int yearStart, int? yearEnd);
        Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear();
        Task<List<HolidayDto>> GetHolidays();
        Task<List<HolidayDto>> GetHolidays(DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> IsHoliday(DateTime date);
        Task<int> GetWorkingDaysBetweenDates(DateTime dateStart, DateTime dateEnd);
        Task<DateTime> AddWorkingDays(DateTime date, int addDays);
        Task<bool> IsHolidayOrWeekend(DateTime date);
    }
}