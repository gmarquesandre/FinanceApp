using AutoMapper;
using FinanceApp.Shared.Dto.ForecastParameters;
using FluentResults;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Implementations
{
    public class ForecastParametersService : CrudSingleBase<ForecastParameters, ForecastParametersDto, CreateOrUpdateForecastParameters> , IForecastParametersService
    {
        public ForecastParametersService(IMapper mapper, IRepository<ForecastParameters> repository) : base(repository, mapper)
        {
        }


        public override async Task<ForecastParametersDto> GetAsync()
        {

            var value = await _repository.FirstOrDefaultAsync();
            if (value != null)
            {
                return new ForecastParametersDto()
                {
                    Id = value.Id,
                    MonthsSavingWarning = value.MonthsSavingWarning,
                    PercentageCdiFixedInteresIncometSavings = value.PercentageCdiFixedInteresIncometSavings,
                    PercentageCdiLoan = value.PercentageCdiLoan,
                    PercentageCdiVariableIncome = value.PercentageCdiVariableIncome,
                    SavingsLiquidPercentage = value.SavingsLiquidPercentage,
                };
                //return _mapper.Map<ForecastParametersDto>(value);
            }
            else
            {
                return new ForecastParametersDto()
                {
                    Id = 0,
                    MonthsSavingWarning = 0,
                    PercentageCdiFixedInteresIncometSavings = 0.00,
                    PercentageCdiLoan = 3.00,
                    PercentageCdiVariableIncome = 0.00,
                    SavingsLiquidPercentage = 0.6
                };
            }
        }
    }
}