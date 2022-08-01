﻿using AutoMapper;
using FinanceApp.Shared.Dto.CurrentBalance;
using FluentResults;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister
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
                await _repository.UpdateAsync(value.Id, model);
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