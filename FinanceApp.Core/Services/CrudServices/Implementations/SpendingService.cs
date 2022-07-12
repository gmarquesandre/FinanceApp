using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class SpendingService : ISpendingService
    {
        private ISpendingForecast _forecast;
        private IRepository<Spending> _repository;
        private IMapper _mapper;
        public SpendingService(IRepository<Spending> repository, IMapper mapper, ISpendingForecast forecast) 
        {

            _forecast = forecast;
        }

        public async Task<SpendingDto> AddAsync(CreateSpending input)
        {
            Spending model = _mapper.Map<Spending>(input);
            CheckValue(model);
            await _repository.InsertAsync(model);
            return _mapper.Map<SpendingDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateSpending input)
        {
            var oldModel = _repository.GetByIdAsync(input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            
            var model = _mapper.Map<Spending>(input);

            CheckValue(model);

            _repository.Update(model.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<List<SpendingDto>> GetAsync()
        {
            var values = await _repository.GetAll()
                .Include(a => a.Category)
                .Include(a => a.CreditCard)                
                .ToListAsync();

            return _mapper.Map<List<SpendingDto>>(values);
        }

        public async Task<SpendingDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);
            return _mapper.Map<SpendingDto>(value);

        }

        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetByIdAsync(id);

            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }
        public async Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth)
        {
            var values = await GetAsync();

            var forecast = _forecast.GetForecast(values, forecastType, maxYearMonth);

            return forecast;
        } 

        public void CheckValue(Spending model)
        {
            if(model.Payment == EPayment.Credit && model.CreditCard == null)
                throw new Exception("Pagamentos em Crédito devem ser vinculados a um cartão");

            if (model.Recurrence == ERecurrence.NTimes && (model.TimesRecurrence == null || model.TimesRecurrence == 0))
                throw new Exception("A quantidade de repetições deve ser maior que zero para o tipo de recorrência selecionado");

            else if (model.Recurrence != ERecurrence.NTimes && model.Recurrence != ERecurrence.Once && model.EndDate == null)
                throw new Exception("A data final deve ser preenchida");

            else if (model.Amount <= 0.00)
                throw new Exception("O valor deve ser maior do que zero");

        }
    }
}