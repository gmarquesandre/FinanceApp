using AutoMapper;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;
using FinanceApp.EntityFramework.User;
using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Entities.UserTables.Bases;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Base
{
    public abstract class CrudServiceBase<T, TDto, TCreate, TUpdate> : ICrudServiceBase<TDto, TCreate, TUpdate>
        where T : UserTable
        where TDto : StandardDto
        where TCreate : CreateDto
        where TUpdate : UpdateDto
    {
        public IRepository<T> _repository;
        public IMapper _mapper;

        public CrudServiceBase(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<Result> UpdateAsync(TUpdate input)
        {
            var entity = _mapper.Map<T>(input);

            entity.CheckInput();

            await _repository.UpdateAsync(entity.Id, entity);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public virtual async Task<TDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(value);
        }

        public virtual async Task<List<TDto>> GetAsync()
        {
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<TDto>>(values);
        }
        public virtual async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetByIdAsync(id);
            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
        public virtual async Task<TDto> AddAsync(TCreate input)
        {
            T entity = _mapper.Map<T>(input);

            entity.CheckInput();

            await _repository.InsertAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

    }
}
