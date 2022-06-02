using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Implementations;
using FinanceApp.Core.Services.Forecast.Base;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.Forecast.Implementations
{
    public class IncomeForecast : BaseForecast
    {
        private readonly IMapper _mapper;
        private readonly IncomeService _service;

        public IncomeForecast(IMapper mapper, IncomeService service)
        {
            _mapper = mapper;
            _service = service;            
        }

        public EItemType Item => EItemType.Income;

        public async Task<List<ForecastItem>> GetMonthlyForecast(CustomIdentityUser user, DateTime maxDate, DateTime? minDate = null)
        {
            decimal cumSum = 0;

            var IncomesSpreadList = await GetIncomesSpreadList(user, maxDate, minDate);

            var monthlyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date.Year, a.Date.Month }, (key, group) =>
              new ForecastItem
              {
                  Amount = group.Sum(a => a.Amount),
                  DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                  CumulatedAmount = 0,
                  Type = Item
              }
            ).ToList();

            monthlyValues.ForEach(a =>
            {
                cumSum += a.Amount;
                a.CumulatedAmount = cumSum;

            });

            return monthlyValues;
        }

        public async Task<List<ForecastItem>> GetDailyForecast(CustomIdentityUser user, DateTime maxDate, DateTime? minDate = null)
        {
            decimal cumSum = 0;

            var IncomesSpreadList = await GetIncomesSpreadList(user, maxDate, minDate);

            var dailyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date }, (key, group) =>
              new ForecastItem
              {
                  Amount = group.Sum(a => a.Amount),
                  DateReference = key.Date,
                  CumulatedAmount = 0,
                  Type = Item
              }
            ).ToList();

            dailyValues.ForEach(a =>
            {
                cumSum += a.Amount;
                a.CumulatedAmount = cumSum;

            });

            return dailyValues;
        }


        public async Task<List<IncomeSpread>> GetIncomesSpreadList(CustomIdentityUser user, DateTime maxYearMonth, DateTime? minYearMonth = null)
        {            
            maxYearMonth = new DateTime(maxYearMonth.Year, maxYearMonth.Month, 1).AddMonths(1).AddDays(-1);
            DateTime minYearMonthUse = minYearMonth ?? DateTime.Now.Date;

            var IncomesDto = await _service.GetAsync(user);

            var IncomeSpreadList = new List<IncomeSpread>();

            foreach (var IncomeDto in IncomesDto)
            {

                if (IncomeDto.Recurrence == ERecurrence.Once)
                {
                    IncomeSpread IncomeSpread = _mapper.Map<IncomeSpread>(IncomeDto);
                    IncomeSpread.Date = IncomeDto.InitialDate;

                    IncomeSpreadList.Add(IncomeSpread);

                }
                else
                {

                    int daysSpanTime = 0;
                    int monthsSpanTime = 0;
                    int yearSpanTime = 0;
                    DateTime? endDate = IncomeDto.EndDate;

                    if (IncomeDto.Recurrence == ERecurrence.Daily)
                    {
                        daysSpanTime = 1;
                    }
                    else if (IncomeDto.Recurrence == ERecurrence.Weekly)
                    {
                        daysSpanTime = 7;
                    }
                    else if (IncomeDto.Recurrence == ERecurrence.Monthly)
                    {
                        monthsSpanTime = 1;
                    }
                    else if (IncomeDto.Recurrence == ERecurrence.Yearly)
                    {
                        yearSpanTime = 1;
                    }


                    if (IncomeDto.TimesRecurrence > 0)
                    {
                        int years = yearSpanTime > 0 ? yearSpanTime * (IncomeDto.TimesRecurrence - 1) : 0;
                        int months = monthsSpanTime > 0 ? monthsSpanTime * (IncomeDto.TimesRecurrence - 1) : 0;
                        int days = daysSpanTime > 0 ? daysSpanTime * (IncomeDto.TimesRecurrence - 1) : 0;

                        endDate = IncomeDto.InitialDate.AddDays(days).AddMonths(months).AddYears(years);

                    }
                    else if (IncomeDto.IsEndless)
                    {
                        endDate = maxYearMonth;
                    }

                    endDate = endDate < maxYearMonth ? endDate : maxYearMonth;

                    DateTime date = IncomeDto.InitialDate;

                    while (date <= endDate)
                    {
                        //ignora datas passadas
                        if (date >= minYearMonthUse)
                        {
                            IncomeSpread IncomeSpread = _mapper.Map<IncomeSpread>(IncomeDto);
                            
                            IncomeSpread.Date = date;
                                                        
                            IncomeSpreadList.Add(IncomeSpread);
                        }

                        try
                        {
                            date = date.AddDays(daysSpanTime).AddMonths(monthsSpanTime).AddYears(yearSpanTime);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            //if date written is like 31 february, does not exist, it will use last day of month
                            date = new DateTime(date.Year, +date.Month, 1).AddDays(-1);

                        }
                    }
                }

            }
            return IncomeSpreadList;

        }        
    }
}
