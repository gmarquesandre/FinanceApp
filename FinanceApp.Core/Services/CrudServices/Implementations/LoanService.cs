using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
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
    public class LoanService : ILoanService
    {
        private ILoanForecast _forecast;
        private IRepository<Loan> _repository;
        private IMapper _mapper;
        public LoanService(FinanceContext context, IMapper mapper, ILoanForecast forecast) {
            _forecast = forecast;            
        }

        public async Task<LoanDto> AddAsync(CreateLoan input)
        {
            Loan model = _mapper.Map<Loan>(input);

            await _repository.InsertAsync(model);
            return _mapper.Map<LoanDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateLoan input)
        {
            var oldModel = _repository.GetById(input.Id);

            var model = _mapper.Map<Loan>(input);
            _repository.Update(model.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<LoanDto> GetAsync(int id)
        {
            var value = await _repository.GetById(id);
            
            return _mapper.Map<LoanDto>(value);

        }

        public async Task<List<LoanDto>> GetAsync()
        {
            var values = await _repository.GetAllAsync();
            return _mapper.Map<List<LoanDto>>(values);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetById(id);
            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }

        public async Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth)
        {
            var values = await GetAsync();

            var forecast = _forecast.GetForecast(values, forecastType, maxYearMonth);

            return forecast;
        }
    }
}