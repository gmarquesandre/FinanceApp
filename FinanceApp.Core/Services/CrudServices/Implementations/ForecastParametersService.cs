using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.ForecastParameters;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class ForecastParametersService : IForecastParametersService
    {
        private IRepository<ForecastParameters> _repository;
        private IMapper _mapper;
        public ForecastParametersService(IMapper mapper, IRepository<ForecastParameters> repository) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ForecastParametersDto> AddOrUpdateAsync(CreateOrUpdateForecastParameters input)
        {
            var value = await _repository.FirstOrDefaultAsync();
            ForecastParameters model = _mapper.Map<ForecastParameters>(input);
            if (value == null)
            {
                await _repository.InsertAsync(model);
            }
            else
            {
                await _repository.UpdateAsync(value.Id, model);
            }
            return _mapper.Map<ForecastParametersDto>(model);
            
        }

        public async Task<ForecastParametersDto> GetAsync()
        {

            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return new ForecastParametersDto()
                {
                    Id = value.Id,
                    MonthsSavingWarning = value.MonthsSavingWarning ?? 0,
                    PercentageCdiFixedInteresIncometSavings = value.PercentageCdiFixedInteresIncometSavings ?? 0,
                    PercentageCdiLoan = value.PercentageCdiLoan ?? 3,
                    PercentageCdiVariableIncome = value.PercentageCdiVariableIncome ?? 0,
                    SavingsLiquidPercentage = value.SavingsLiquidPercentage ?? 0,
                };
                //return _mapper.Map<ForecastParametersDto>(value);
            }
            else
            {
                return new ForecastParametersDto()
                {
                    Id = 0,
                    MonthsSavingWarning = 0,
                    PercentageCdiFixedInteresIncometSavings = 1,
                    PercentageCdiLoan = 3,
                    PercentageCdiVariableIncome = 1,
                    SavingsLiquidPercentage = 0.6                    
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