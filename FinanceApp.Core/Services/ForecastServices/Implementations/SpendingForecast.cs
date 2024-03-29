﻿using AutoMapper;
using FinanceApp.Core.Services.ForecastServices.Base;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Implementations
{
    public class SpendingForecast : BaseForecast, ISpendingForecast
    {
        private readonly IMapper _mapper;

        public SpendingForecast(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EItemType Item => EItemType.Spending;
        public ForecastList GetForecast(List<SpendingDto> spendingDtos, EForecastType forecastType, DateTime maxDate, DateTime minDate)
        {

            if (forecastType == EForecastType.Daily)
                return GetDailyForecast(spendingDtos, maxDate, minDate);
            else if (forecastType == EForecastType.Monthly)
                return GetMonthlyForecast(spendingDtos, maxDate, minDate);

            throw new Exception("Tipo de previsão inválido");

        }
        private ForecastList GetMonthlyForecast(List<SpendingDto> spendingDtos, DateTime maxDate, DateTime minDate)
        {
            double cumSum = 0;

            var spendingsSpreadList = GetSpendingsSpreadList(spendingDtos, maxDate, minDate);

            var monthlyValues = spendingsSpreadList.OrderBy(a => a.ReferenceDate).GroupBy(a => new { a.ReferenceDate.Year, a.ReferenceDate.Month }, (key, group) =>
              new ForecastItem
              {
                  NominalLiquidValue = group.Sum(a => a.Amount),
                  DateReference = new DateTime(key.Year, key.Month, 1).AddMonths(1).AddDays(-1),
                  NominalCumulatedAmount = 0
              }
            ).ToList();

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
        private ForecastList GetDailyForecast(List<SpendingDto> spendingDtos, DateTime maxDate, DateTime minDate)
        {
            double cumSum = 0;
            var spendingsSpreadList = GetSpendingsSpreadList(spendingDtos, maxDate, minDate);

            var dailyValues = spendingsSpreadList.OrderBy(a => a.ReferenceDate).GroupBy(a => new { a.ReferenceDate }, (key, group) =>
              new ForecastItem
              {
                  NominalLiquidValue = group.Sum(a => a.Amount),
                  DateReference = key.ReferenceDate,
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
                Items = dailyValues,
                Type = Item
            };

        }
        private List<SpendingSpread> GetSpendingsSpreadList(List<SpendingDto> spendingsDto, DateTime maxDate, DateTime minDate)
        {
            var spendingSpreadList = new List<SpendingSpread>();

            foreach (var spendingDto in spendingsDto)
            {
                spendingSpreadList.AddRange(GetSpendingList(maxDate, minDate, spendingDto));

            }

            return spendingSpreadList.Where(a => a.ReferenceDate >= minDate && a.ReferenceDate <= maxDate).ToList();

        }

        public List<SpendingSpread> GetSpendingList(DateTime maxYearMonth, DateTime minDate, SpendingDto spendingDto)
        {
            List<SpendingSpread> spendingSpreadList = new();
            if (spendingDto.Recurrence == ERecurrence.Once)
            {
                SpendingSpread spendingSpread = _mapper.Map<SpendingSpread>(spendingDto);
                spendingSpread.Date = spendingDto.InitialDate;

                spendingSpreadList.Add(spendingSpread);

            }
            else
            {

                int daysSpanTime = 0;
                int monthsSpanTime = 0;
                int yearSpanTime = 0;
                DateTime? endDate = spendingDto.EndDate;

                if (spendingDto.Recurrence == ERecurrence.Daily)
                {
                    daysSpanTime = 1;
                }
                else if (spendingDto.Recurrence == ERecurrence.Weekly)
                {
                    daysSpanTime = 7;
                }
                else if (spendingDto.Recurrence == ERecurrence.Monthly)
                {
                    monthsSpanTime = 1;
                }
                else if (spendingDto.Recurrence == ERecurrence.Yearly)
                {
                    yearSpanTime = 1;
                }
                else
                {
                    throw new Exception("Recorrencia não encontrada");
                };


                if (spendingDto.TimesRecurrence > 0)
                {
                    int years = yearSpanTime > 0 ? yearSpanTime * (spendingDto.TimesRecurrence - 1) : 0;
                    int months = monthsSpanTime > 0 ? monthsSpanTime * (spendingDto.TimesRecurrence - 1) : 0;
                    int days = daysSpanTime > 0 ? daysSpanTime * (spendingDto.TimesRecurrence - 1) : 0;

                    endDate = spendingDto.InitialDate.AddDays(days).AddMonths(months).AddYears(years);

                }
                else if (spendingDto.IsEndless)
                {
                    endDate = maxYearMonth;
                }

                endDate = endDate < maxYearMonth ? endDate : maxYearMonth;

                DateTime date = spendingDto.InitialDate;

                while (date <= endDate)
                {
                    //ignora datas passadas
                    if (date >= minDate)
                    {
                        SpendingSpread spendingSpread = _mapper.Map<SpendingSpread>(spendingDto);
                        if (spendingDto.Payment == EPayment.Cash)
                        {
                            spendingSpread.Date = date;
                            spendingSpread.ReferenceDate = date;

                        }
                        else if (spendingDto.Payment == EPayment.Credit)
                        {
                            spendingSpread.ReferenceDate = CheckDateForCredit(date, spendingDto.CreditCard!);

                            spendingSpread.Date = date;
                        }

                        spendingSpreadList.Add(spendingSpread);
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

            return spendingSpreadList;
        }

        private static DateTime CheckDateForCredit(DateTime date, CreditCardDto creditCard)
        {
            if (date >= new DateTime(date.Year, date.Month, creditCard.InvoiceClosingDay))
            {
                return new DateTime(date.Year, date.Month, creditCard.InvoicePaymentDay).AddMonths(1);
            }
            else
            {
                return new DateTime(date.Year, date.Month, creditCard.InvoicePaymentDay);
            }
        }
    }
}
