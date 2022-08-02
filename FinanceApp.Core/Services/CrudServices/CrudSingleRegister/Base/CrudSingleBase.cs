using AutoMapper;
using FinanceApp.EntityFramework.User;
using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Entities.UserTables.Bases;
using FluentResults;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base
{
    public abstract class CrudSingleBase<T, TDto, TCreateOrUpdate> : ICrudSingleBase<TDto, TCreateOrUpdate>
        where T : UserTable
        where TDto : StandardDto
        where TCreateOrUpdate : CreateOrUpdateDto
    {
        public IRepository<T> _repository;
        public IMapper _mapper;

        public CrudSingleBase(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public abstract Task<TDto> GetAsync();

        public async Task<TDto> AddOrUpdateAsync(TCreateOrUpdate input)
        {
            var value = await _repository.FirstOrDefaultAsync();
            T model = _mapper.Map<T>(input);

            model.CheckInput();

            if (value == null)
            {
                await _repository.InsertAsync(model);
            }
            else
            {
                await _repository.UpdateAsync(value.Id, model);
            }
            return _mapper.Map<TDto>(model);

        }

        public async Task<Result> DeleteAsync()
        {
            var investment = await _repository.FirstOrDefaultAsync();

            if (investment == null)
                throw new Exception("Não encotrado");

            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }

    }
}
