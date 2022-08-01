﻿using FinanceApp.Shared.Dto.Base;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.Base
{
    public interface ICommand<TDto, TCreate, TUpdate> 
        where TDto : StandardDto 
        where TCreate : CreateDto
        where TUpdate : UpdateDto
        
    {
        Task<TDto> AddAsync(TCreate input);
        Task<Result> DeleteAsync(int id);
        Task<Result> UpdateAsync(TUpdate input);
    }
}
