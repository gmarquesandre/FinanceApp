using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class IncomeService : IIncomeService
    {
        private IRepository<Income> _repository;
        private IMapper _mapper;
        private IIncomeForecast _forecast;
        public IncomeService(IRepository<Income> repository, IMapper mapper, IIncomeForecast forecast)
        {
            _repository = repository;
            _mapper = mapper;
            _forecast = forecast;
        }

        public async Task<IncomeDto> AddAsync(CreateIncome input)
        {

            Income model = _mapper.Map<Income>(input);

            CheckValue(model);

            await _repository.InsertAsync(model);
            return _mapper.Map<IncomeDto>(model);

        }
        public async Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth)
        {
            var values = await GetAsync();

            var forecast = _forecast.GetForecast(values, forecastType, maxYearMonth);

            return forecast;
        }

        public async Task<Result> UpdateAsync(UpdateIncome input)
        {
            var oldModel = await _repository.GetByIdAsync(input.Id);

            if (oldModel == null)
                return Result.Fail("Não Encontrado");

            var model = _mapper.Map<Income>(input);

            CheckValue(model);

            await _repository.UpdateAsync(oldModel.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }



        public async Task<List<IncomeDto>> GetAsync()
        {
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<IncomeDto>>(values);
        }

        public async Task<IncomeDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<IncomeDto>(value);

        }

        public async Task<Result> DeleteAsync(int id)
        {
            var income = await _repository.GetByIdAsync(id);

            if (income == null)
            {
                return Result.Fail("Não Encontrado");
            }

            _repository.Remove(income);
            return Result.Ok().WithSuccess("Investimento deletado");
        }


        public void CheckValue(Income model)
        {
            if (model.Recurrence != ERecurrence.Once && model.EndDate == null && !model.IsEndless && (model.TimesRecurrence == null || model.TimesRecurrence == 0))
                throw new Exception("Inválido");

            else if (model.Amount <= 0.00)
                throw new Exception("O valor deve ser maior do que zero");

        }
    }
}