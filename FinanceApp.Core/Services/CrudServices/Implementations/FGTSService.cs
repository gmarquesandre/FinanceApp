﻿using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class FGTSService : IFGTSService
    {
        private IRepository<FGTS> _repository;
        private IMapper _mapper;
        public FGTSService(IRepository<FGTS> repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input)
        {
            var value = await _repository.FirstOrDefaultAsync();
            FGTS model = _mapper.Map<FGTS>(input);
            if (value == null)
            {
                await _repository.InsertAsync(model);
            }
            else
            {
                _repository.Update(value.Id, model);
            }
            return _mapper.Map<FGTSDto>(model);

        }

        public async Task<FGTSDto> GetAsync()
        {
            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<FGTSDto>(value);
            }
            else
            {
                return new FGTSDto()
                {
                    AnniversaryWithdraw = false,
                    CurrentBalance = 0.00,
                    MonthlyGrossIncome = 0.00                     
                };
            }
        }


        public async Task<Result> DeleteAsync()
        {
            var investment = await _repository.FirstOrDefaultAsync();
            
            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}