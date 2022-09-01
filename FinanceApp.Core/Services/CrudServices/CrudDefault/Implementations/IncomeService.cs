using AutoMapper;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class IncomeService :
        CrudBase<Income, IncomeDto, CreateIncome, UpdateIncome>,         
        IIncomeService
    {
        public IIncomeForecast _forecast { get; set; }
        public IncomeService(IRepository<Income> repository, IMapper mapper, IIncomeForecast forecast) : base(repository, mapper)
        {
            _forecast = forecast;
        }
       
        public async Task<ForecastList> GetForecast(EForecastType type, DateTime maxYearMonth, DateTime currentDate)
        {
            var dtos = await GetAsync();
            var values = _forecast.GetForecast(dtos, type, maxYearMonth, currentDate);
            return values;
        }
    }

}