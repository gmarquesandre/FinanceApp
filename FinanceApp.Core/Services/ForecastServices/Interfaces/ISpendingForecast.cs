﻿using FinanceApp.Shared;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices.Interfaces
{
    public interface ISpendingForecast 
    {
        EItemType Item { get; }
        ForecastList GetForecast(List<SpendingDto> spendingDtos, EForecastType forecastType, DateTime maxDate, DateTime minDate);
    }
}