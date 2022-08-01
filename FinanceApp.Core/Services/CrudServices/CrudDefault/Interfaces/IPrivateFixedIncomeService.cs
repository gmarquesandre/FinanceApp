﻿using FinanceApp.Shared;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces
{
    public interface IPrivateFixedIncomeService
    {
        Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input);
        Task<Result> DeleteAsync(int id);
        Task<List<PrivateFixedIncomeDto>> GetAsync();
        Task<PrivateFixedIncomeDto> GetAsync(int id);
        Task<Result> UpdateAsync(UpdatePrivateFixedIncome input);
    }
}