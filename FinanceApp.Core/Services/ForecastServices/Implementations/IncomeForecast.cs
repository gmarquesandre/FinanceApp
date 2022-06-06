using AutoMapper;
using FinanceApp.Core.Services.ForecastServices.Base;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public class IncomeForecast : BaseForecast
    {
        private readonly IMapper _mapper;

        public IncomeForecast(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EItemType Item => EItemType.Income;

        public List<ForecastItem> GetMonthlyForecast(List<IncomeDto> incomes, DateTime maxDate, DateTime? minDate = null)
        {
            decimal cumSum = 0;

            var IncomesSpreadList = GetIncomesSpreadList(incomes, maxDate, minDate);

            var monthlyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date.Year, a.Date.Month }, (key, group) =>
              new ForecastItem
              {
                  Amount = group.Sum(a => a.Amount),
                  DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                  CumulatedAmount = 0
              }
            ).ToList();

            monthlyValues.ForEach(a =>
            {
                cumSum += a.Amount;
                a.CumulatedAmount = cumSum;

            });

            return monthlyValues;
        }

        public List<ForecastItem> GetDailyForecast(List<IncomeDto> incomesDto, DateTime maxDate, DateTime? minDate = null)
        {
            decimal cumSum = 0;

            var IncomesSpreadList = GetIncomesSpreadList(incomesDto, maxDate, minDate);

            var dailyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date }, (key, group) =>
              new ForecastItem
              {
                  Amount = group.Sum(a => a.Amount),
                  DateReference = key.Date,
                  CumulatedAmount = 0
              }
            ).ToList();

            dailyValues.ForEach(a =>
            {
                cumSum += a.Amount;
                a.CumulatedAmount = cumSum;

            });

            return dailyValues;
        }


        public List<IncomeSpread> GetIncomesSpreadList(List<IncomeDto> incomesDto, DateTime maxYearMonth, DateTime? minDateInput = null)
        {
            maxYearMonth = new DateTime(maxYearMonth.Year, maxYearMonth.Month, 1).AddMonths(1).AddDays(-1);
            DateTime minDate = minDateInput ?? DateTime.Now.Date;

            var incomeSpreadList = new List<IncomeSpread>();

            foreach (var IncomeDto in incomesDto)
            {

                if (IncomeDto.Recurrence == ERecurrence.Once)
                {
                    IncomeSpread IncomeSpread = _mapper.Map<IncomeSpread>(IncomeDto);
                    IncomeSpread.Date = IncomeDto.InitialDate;

                    incomeSpreadList.Add(IncomeSpread);

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
                        if (date >= minDate)
                        {
                            IncomeSpread IncomeSpread = _mapper.Map<IncomeSpread>(IncomeDto);

                            IncomeSpread.Date = date;

                            incomeSpreadList.Add(IncomeSpread);
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
            return incomeSpreadList;

        }
    }
}