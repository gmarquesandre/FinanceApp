using AutoMapper;
using FinanceApp.Api;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FinanceApp.Core.Services
{
    public class DatesService : ServiceBase, IDatesService
    {
        private FinanceContext _context;
        private IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public DatesService(FinanceContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear()
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.WorkingDays);

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<WorkingDaysByYearDto> valuesDto))
            {
                var values = await _context.WorkingDaysByYear.ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                valuesDto = _mapper.Map<List<WorkingDaysByYearDto>>(values);

                //setting cache entries
                _memoryCache.Set(cacheKey, values, cacheExpiryOptions);
            }

            return valuesDto;
        }

        public async Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear(int yearStart, int? yearEnd)
        {
            var values = await GetWorkingDaysByYear();

            if (yearEnd.HasValue)
                return values.Where(a => a.Year >= yearStart && a.Year <= yearEnd).ToList();

            return values.Where(a => a.Year >= yearStart).ToList();
        }
        public async Task<WorkingDaysByYearDto> GetWorkingDaysOfAYear(int year)
        {
            var values = await GetWorkingDaysByYear();

            var value = values.FirstOrDefault(a => a.Year == year);

            if (value == null)
                return new WorkingDaysByYearDto() { WorkingDays = 252, Year = year};

            return value;

        }
        public async Task<List<HolidayDto>> GetHolidays()
        {
            var cacheKey = EnumHelper<EDataCacheKey>.GetDescriptionValue(EDataCacheKey.Holidays);

            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<HolidayDto> valuesDto))
            {
                var values = await _context.Holidays.ToListAsync();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromHours(1)
                };

                valuesDto = _mapper.Map<List<HolidayDto>>(values);

                //setting cache entries
                _memoryCache.Set(cacheKey, values, cacheExpiryOptions);
            }

            return valuesDto;
        }
        public async Task<bool> IsHoliday(DateTime date)
        {
            var holidays = await GetHolidays();

            return holidays.Where(a => a.Date == date).Any();

        }

        public async Task<bool> IsHolidayOrWeekend(DateTime date)
        {
            var holidays = await GetHolidays();

            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                return true;

            return holidays.Where(a => a.Date == date).Any();

        }

        public async Task<List<HolidayDto>> GetHolidays(DateTime? startDate = null, DateTime? endDate = null)
        {
            var values = await GetHolidays();

            if (startDate.HasValue && endDate.HasValue)
                return values.Where(a => a.Date >= startDate.Value && a.Date <= endDate.Value).ToList();

            else if (startDate.HasValue)
                return values.Where(a => a.Date >= startDate).ToList();

            else if (endDate.HasValue)
                return values.Where(a => a.Date <= endDate.Value).ToList();

            return values;
        }
        public async Task<int> GetWorkingDaysBetweenDates(DateTime dateStart, DateTime dateEnd)
        {
            var holidays = await GetHolidays(dateStart, dateEnd);

            var holidaysOnWorkingDays = holidays
                .Where(a => a.Date.DayOfWeek != DayOfWeek.Sunday && a.Date.DayOfWeek != DayOfWeek.Saturday)
                .ToList();

            int holidaysCountBetweenDates = holidaysOnWorkingDays.Count;

            double calcWorkingDaysDays =
                1 + ((dateEnd - dateStart).TotalDays * 5 -
                (dateStart.DayOfWeek - dateEnd.DayOfWeek) * 2) / 7;

            if (dateEnd.DayOfWeek == DayOfWeek.Saturday) calcWorkingDaysDays--;
            if (dateStart.DayOfWeek == DayOfWeek.Sunday) calcWorkingDaysDays--;

            return Convert.ToInt32(calcWorkingDaysDays) - holidaysCountBetweenDates;
        }


        public async Task<DateTime> AddWorkingDays(DateTime date, int addDays)
        {
            if(addDays > 0)
            {
                int days = 0;
                int addDaysTest = addDays - 1;

                while(days < addDays)
                {
                    addDaysTest++;

                    days = await GetWorkingDaysBetweenDates(date, date.AddDays(addDaysTest)) - 1;

                }

                return date.AddDays(addDaysTest);
            }
            else if(addDays < 0)
            {
                int days = 1;
                int addDaysTest = addDays + 1;

                while (days > addDays)
                {

                    addDaysTest--;

                    days = await GetWorkingDaysBetweenDates(date.AddDays(addDaysTest), date) * -1 + 1;

                }

                return date.AddDays(addDaysTest);
            }
            
            return date;
            
        }

    }
}
