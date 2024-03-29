﻿using FinanceApp.Shared.Dto.Base;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces
{
    public interface ICommand<TDto, TCreate, TUpdate>
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto

    {
        Task<TDto> AddAsync(TCreate input);
        Task<Result> DeleteAsync(Guid id);
        Result DeleteBatch(List<Guid> ids);
        Task<Result> DeleteAllAsync();
        Task<Result> UpdateAsync(TUpdate input);
    }
}
