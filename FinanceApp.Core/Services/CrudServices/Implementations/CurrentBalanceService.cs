using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FinanceApp.EntityFramework;
using FinanceApp.Shared;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class CurrentBalanceService : ICurrentBalanceService
    {
        private IRepository<CurrentBalance> _repository;
        private IMapper _mapper;
        public CurrentBalanceService(IMapper mapper, IRepository<CurrentBalance> repository) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input)
        {
            var value = await _repository.FirstOrDefaultAsync();
            CurrentBalance model = _mapper.Map<CurrentBalance>(input);
            if (value == null)
            {
                await _repository.InsertAsync(model);
            }
            else
            {
                _repository.Update(value.Id, model);
            }
            return _mapper.Map<CurrentBalanceDto>(model);
            
        }

        public async Task<CurrentBalanceDto> GetAsync()
        {

            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<CurrentBalanceDto>(value);
            }
            else
            {
                return new CurrentBalanceDto()
                {
                    Id = 0,
                    PercentageCdi = null,
                    UpdateValueWithCdiIndex = false,
                    Value = 0.00,
                    CreationDateTime = new DateTime(1900,1,1),
                    UpdateDateTime = new DateTime(1900, 1, 1),

                };
            }
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