using AutoMapper;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Base;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Core.Services.ForecastServices.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Implementations
{
    public class FGTSService : CrudSingleBase<FGTS, FGTSDto, CreateOrUpdateFGTS>, IFGTSService
    {
        public IFGTSForecast _forecast { get; set; }
        public FGTSService(IRepository<FGTS> repository, IMapper mapper, IFGTSForecast forecast) : base(repository, mapper)
        {
            _forecast = forecast;
        }
        public async Task<ForecastList> GetForecast(EForecastType type, DateTime maxYearMonth, DateTime currentDate)
        {
            var dtos = await GetAsync();
            var values = _forecast.GetForecastAsync(dtos, type, maxYearMonth, currentDate);
            return values;
        }

        public override async Task<FGTSDto> GetAsync()
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
                    Id = new Guid(),
                    AnniversaryWithdraw = false,
                    CurrentBalance = 0.00,
                    MonthlyGrossIncome = 0.00,
                    MonthAniversaryWithdraw = 1
                    
                };
            }
        }
    }
}