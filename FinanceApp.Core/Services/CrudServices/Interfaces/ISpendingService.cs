﻿using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Interfaces
{
    public interface ISpendingService : ITransientService
    {
        Task<SpendingDto> AddAsync(CreateSpending input, CustomIdentityUser user);
        Task<Result> DeleteAsync(int id, CustomIdentityUser user);
        Task<List<SpendingDto>> GetAsync(CustomIdentityUser user);
        Task<SpendingDto> GetAsync(CustomIdentityUser user, int id);
        Task<Result> UpdateAsync(UpdateSpending input, CustomIdentityUser user);
        Task<ForecastList> GetForecast(CustomIdentityUser user, EForecastType forecastType, DateTime maxYearMonth);
    }
}