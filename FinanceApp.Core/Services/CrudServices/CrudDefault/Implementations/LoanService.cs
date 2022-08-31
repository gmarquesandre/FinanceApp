using AutoMapper;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class LoanService : CrudBase<Loan, LoanDto, CreateLoan, UpdateLoan>, ILoanService
    {
        public ILoanForecast _forecast { get; set; }
        public LoanService(IRepository<Loan> repository, IMapper mapper, ILoanForecast forecast) : base(repository, mapper)
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