﻿using FinanceApp.Shared.Dto.Base;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface ICommand<TDto, TCreate, TUpdate>
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto

    {
        Task<Result> AddAsync(TCreate input);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateAsync(TUpdate input);
    }
}
