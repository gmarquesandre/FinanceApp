using AutoMapper;
using FinanceApp.Shared.Dto.Spending;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared.Entities.UserTables;
using FinanceApp.EntityFramework.User;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Base;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Core.Services.ForecastServices.Interfaces;

namespace FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations
{
    public class SpendingService : CrudBase<Spending, SpendingDto, CreateSpending, UpdateSpending>, ISpendingService
    {
        private ISpendingForecast _forecast { get; set; }
        public SpendingService(IRepository<Spending> repository, IMapper mapper, ISpendingForecast forecast) : base(repository, mapper) {
            _forecast = forecast;
        }

        public override async Task<List<SpendingDto>> GetAsync()
        {
            var values = await _repository.GetAllAsync()
                .Include(a => a.Category)
                .Include(a => a.CreditCard)
                .ToListAsync();

            return _mapper.Map<List<SpendingDto>>(values);
        }

        public override async Task<SpendingDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return _mapper.Map<SpendingDto>(value);

        }

        public async Task<ForecastList> GetForecast(EForecastType type, DateTime maxDate, DateTime currentDate)
        {
            var dtos = await GetAsync();
            var values = _forecast.GetForecast(dtos, type, maxDate, currentDate);
            return values;
        }
    }
}