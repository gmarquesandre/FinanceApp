﻿using AutoMapper;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.EntityFramework.User;
using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Entities.UserTables;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base
{
    public abstract class CrudBase<T, TDto, TCreate, TUpdate> : ICrudBase<TDto, TCreate, TUpdate>
        where T : UserTable
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto
    {
        public IRepository<T> _repository;
        public IMapper _mapper;

        public CrudBase(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<Result> UpdateAsync(TUpdate input)
        {
            var entity = _mapper.Map<T>(input);

            entity.CheckInput();

            await _repository.UpdateAsync(entity.Id, entity);
            return Result.Ok().WithSuccess("Registro atualizado com sucesso");
        }

        public virtual async Task<TDto> GetAsync(Guid id)
        {
            var value = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(value);
        }

        public virtual async Task<List<TDto>> GetAsync()
        {
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<TDto>>(values);
        }
        public virtual async Task<Result> DeleteAsync(Guid id)
        {
            var investment = await _repository.GetByIdAsync(id);
            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Registro deletado com sucesso");
        }
        public virtual async Task<TDto> AddAsync(TCreate input)
        {
            T entity = _mapper.Map<T>(input);

            entity.CheckInput();

            var createdEntity = await _repository.InsertAsync(entity);

            TDto newEntity = _mapper.Map<TDto>(createdEntity);

            return newEntity;
        }

        public Result DeleteBatch(List<Guid> ids)
        {
            _repository.RemoveBatch(ids);

            return Result.Ok().WithSuccess("Registros deletados com sucesso");
        }

        public async Task<Result> DeleteAllAsync()
        {
            await _repository.RemoveAll();

            return Result.Ok().WithSuccess("Registros deletados com sucesso");

        }
    }
}
