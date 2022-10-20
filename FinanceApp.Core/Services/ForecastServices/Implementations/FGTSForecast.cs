using AutoMapper;
using FinanceApp.Core.Services.ForecastServices.Base;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public class FGTSForecast : BaseForecast, IFGTSForecast
    {
        private readonly IMapper _mapper;

        public FGTSForecast(IMapper mapper)
        {
            _mapper = mapper;
        }

        private static readonly List<FGTSAnniversaryWithdraw> FgtsWithdrawValueList = new() { 
            //1
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 0.00,
                AdditionalAmount = 0,
                MaxValue = 500.00,
                WithdrawPercentage = 0.5
            },
            //2
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 500.01,
                AdditionalAmount = 50,
                MaxValue = 1000.00,
                WithdrawPercentage = 0.4
            },
            //3
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 1000.01,
                AdditionalAmount = 150,
                MaxValue = 5000,
                WithdrawPercentage = 0.3
            },
            //4
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 5000.01,
                AdditionalAmount = 650.00,
                MaxValue = 10000.00,
                WithdrawPercentage = 0.2
            },
            //5
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 10000.01,
                AdditionalAmount = 1150.00,
                MaxValue = 15000.00,
                WithdrawPercentage = 0.25
            },
            //6
            new FGTSAnniversaryWithdraw()
            {
                MinValue = 15000.01,
                AdditionalAmount = 1900.00,
                MaxValue = 20000.0,
                WithdrawPercentage = 0.1
            },
            //7
              new FGTSAnniversaryWithdraw()
            {
                MinValue = 20000.01,
                AdditionalAmount = 2900.00,
                MaxValue = double.MaxValue,
                WithdrawPercentage = 0.05
            },
        };


        public EItemType Item => EItemType.FGTS;
        public ForecastList GetForecastAsync(FGTSDto fgtsDto, EForecastType forecastType, DateTime maxDate, DateTime? minDate = null)
        {

            if (forecastType == EForecastType.Daily)
                return GetDailyForecastAsync(fgtsDto, maxDate, minDate);
            else if (forecastType == EForecastType.Monthly)
                return GetMonthlyForecastAsync(fgtsDto, maxDate, minDate);

            throw new Exception("Tipo de previsão inválido");

        }
        private ForecastList GetMonthlyForecastAsync(FGTSDto fgtsDto, DateTime maxDate, DateTime? minDate = null)
        {
            var FGTSsSpreadList = GetFGTSsSpreadListAsync(fgtsDto, maxDate, minDate);

            var monthlyValues = FGTSsSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date.Year, a.Date.Month }, (key, group) =>
              new ForecastItem
              {
                  NominalNotLiquidValue = group.Sum(a => a.AddValue),
                  NominalLiquidValue = group.Sum(a => a.WithdrawValue),
                  DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                  NominalCumulatedAmount = group.Sum(a => a.CurrentBalance),
              }
            ).OrderBy(a => a.DateReference).ToList();



            return new ForecastList()
            {
                Type = Item,
                Items = monthlyValues
            };
        }

        private ForecastList GetDailyForecastAsync(FGTSDto incomesDto, DateTime maxDate, DateTime? minDate = null)
        {
            var FGTSsSpreadList = GetFGTSsSpreadListAsync(incomesDto, maxDate, minDate);

            var dailyValues = FGTSsSpreadList.OrderBy(a => a.Date).GroupBy(a => new { a.Date }, (key, group) =>
              new ForecastItem
              {
                  NominalNotLiquidValue = group.Sum(a => a.CurrentBalance),
                  NominalLiquidValue = group.Sum(a => a.WithdrawValue),
                  DateReference = key.Date,
                  NominalCumulatedAmount = group.Sum(a => a.CurrentBalance)
              }).ToList();            

            return new ForecastList()
            {
                Type = Item,
                Items = dailyValues
            };
        }


        public List<FGTSSpread> GetFGTSsSpreadListAsync(FGTSDto fgtsDto, DateTime maxYearMonth, DateTime? minDateInput = null)
        {
            maxYearMonth = maxYearMonth.GetLastDayOfThisMonth();
            DateTime minDate = minDateInput ?? DateTime.Now.Date.AddDays(1);

            return GenerateFGTSListAsync(fgtsDto, maxYearMonth, minDate);

        }

        private List<FGTSSpread> GenerateFGTSListAsync(FGTSDto fgtsDto, DateTime maxYearMonth, DateTime minDate)
        {
            List<FGTSSpread> fgtsSpreadList = new();

            double fgtsInterestRateMonthly = GlobalVariables.FgtsMonthlyInterestRate;
            double monthlyDeposit = fgtsDto.MonthlyGrossIncome * GlobalVariables.FgtsMonthlyGrossIncomePercentageDeposit;

            var currentMonth = _mapper.Map<FGTSSpread>(fgtsDto);

            currentMonth.Date = currentMonth.Date.GetLastDayOfThisMonth();
            currentMonth.ReferenceDate = currentMonth.Date;
            fgtsSpreadList.Add(currentMonth);

            for (DateTime date = fgtsDto.UpdateDateTime.AddMonths(1); date <= maxYearMonth; date = date.AddMonths(1))
            {
                var newItem = _mapper.Map<FGTSSpread>(fgtsDto);
                newItem.Date = date.GetLastDayOfThisMonth();                
                newItem.ReferenceDate = newItem.Date;

                if (fgtsDto.AnniversaryWithdraw && fgtsDto.MonthAniversaryWithdraw == date.Month)
                {
                    var withdraw = WithdrawValue(newItem);

                    newItem.CurrentBalance -= withdraw;

                    FGTSSpread withdrawObject = new()
                    {
                        Date = newItem.Date.GetFirstDayOfThisMonth(),
                        AnniversaryWithdraw = newItem.AnniversaryWithdraw,
                        CurrentBalance = 0,
                        AddValue = 0,
                        WithdrawValue = withdraw,
                        MonthAniversaryWithdraw = newItem.MonthAniversaryWithdraw,
                        MonthlyGrossIncome = newItem.MonthlyGrossIncome,
                        Id = newItem.Id,
                        ReferenceDate = newItem.ReferenceDate.GetFirstDayOfThisMonth(),
                        UpdateDateTime = newItem.UpdateDateTime

                    };

                    fgtsSpreadList.Add(withdrawObject);


                }
                newItem.AddValue = newItem.CurrentBalance * fgtsInterestRateMonthly + monthlyDeposit;
                newItem.CurrentBalance += newItem.AddValue;
                fgtsDto = newItem;
                fgtsSpreadList.Add(newItem);
            }

            fgtsSpreadList = fgtsSpreadList.Where(a => a.Date >= minDate).ToList();

            return fgtsSpreadList;
        }

        private static double WithdrawValue(FGTSSpread newItem)
        {
            var withdrawParameters = FgtsWithdrawValueList.FirstOrDefault(a => newItem.CurrentBalance >= a.MinValue && newItem.CurrentBalance <= a.MaxValue)!;

            double withdrawValue = newItem.CurrentBalance * withdrawParameters.WithdrawPercentage + withdrawParameters.AdditionalAmount;
            return withdrawValue;
        }
    }
}