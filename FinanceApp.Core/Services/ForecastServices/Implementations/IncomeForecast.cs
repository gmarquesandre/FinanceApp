using AutoMapper;
using FinanceApp.Core.Services.ForecastServices.Base;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public class IncomeForecast : BaseForecast, IIncomeForecast
    {
        private readonly IMapper _mapper;

        public IncomeForecast(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EItemType Item => EItemType.Income;
        public ForecastList GetForecast(List<IncomeDto> incomeDtos, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null)
        {

            if (forecastType == EForecastType.Daily)
                return GetDailyForecast(incomeDtos, maxDate, minDate);
            else if (forecastType == EForecastType.Monthly)
                return GetMonthlyForecast(incomeDtos, maxDate, minDate);

            throw new Exception("Tipo de previsão inválido");

        }
        private ForecastList GetMonthlyForecast(List<IncomeDto> incomes, DateTime maxDate, DateTime? minDate = null)
        {
            double cumSum = 0;

            var IncomesSpreadList = GetIncomesSpreadList(incomes, maxDate, minDate);

            var monthlyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date.Year, a.Date.Month }, (key, group) =>
              new ForecastItem
              {
                  NominalLiquidValue = group.Sum(a => a.Amount),
                  DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                  NominalCumulatedAmount = 0
              }
            ).OrderBy(a=> a.DateReference).ToList();

            monthlyValues.ForEach(a =>
            {
                cumSum += a.NominalLiquidValue;
                a.NominalCumulatedAmount = cumSum;

            });

            return new ForecastList()
            {
                Type = Item,
                Items = monthlyValues
            };
        }

        private ForecastList GetDailyForecast(List<IncomeDto> incomesDto, DateTime maxDate, DateTime? minDate = null)
        {
            double cumSum = 0;

            var IncomesSpreadList = GetIncomesSpreadList(incomesDto, maxDate, minDate);

            var dailyValues = IncomesSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date }, (key, group) =>
              new ForecastItem
              {
                  NominalLiquidValue = group.Sum(a => a.Amount),
                  DateReference = key.Date,
                  NominalCumulatedAmount = 0
              }
            ).ToList();

            dailyValues.ForEach(a =>
            {
                cumSum += a.NominalLiquidValue;
                a.NominalCumulatedAmount = cumSum;

            });
            return new ForecastList()
            {
                Type = Item,
                Items = dailyValues
            };
        }


        public List<IncomeSpread> GetIncomesSpreadList(List<IncomeDto> incomesDto, DateTime maxYearMonth, DateTime? minDateInput = null)
        {
            maxYearMonth = maxYearMonth.GetLastDayOfThisMonth();
            DateTime minDate = minDateInput ?? DateTime.Now.Date.AddDays(1);

            var incomeSpreadList = new List<IncomeSpread>();

            foreach (var IncomeDto in incomesDto)
            {
                incomeSpreadList.AddRange(GetIncomeList(maxYearMonth, minDate, IncomeDto));

            }
            return incomeSpreadList;
        }

        public List<IncomeSpread> GetIncomeList(DateTime maxYearMonth, DateTime minDate, IncomeDto IncomeDto)
        {
            List<IncomeSpread> incomeSpreadList = new();

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

            return incomeSpreadList;
        }
    }
}