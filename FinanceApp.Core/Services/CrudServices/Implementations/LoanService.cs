using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class LoanService : ILoanService
    {
        private ILoanForecast _forecast;
        private IRepository<Loan> _repository;
        private IMapper _mapper;
        public LoanService(IRepository<Loan> repository, IMapper mapper, ILoanForecast forecast) {            
            _forecast = forecast;            
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LoanDto> AddAsync(CreateLoan input)
        {
            Loan model = _mapper.Map<Loan>(input);
            await _repository.InsertAsync(model);
            return _mapper.Map<LoanDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateLoan input)
        {            
            var model = _mapper.Map<Loan>(input);
            await _repository.UpdateAsync(model.Id, model);
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<LoanDto> GetAsync(int id)
        {
            var value = await _repository.GetByIdAsync(id);            
            return _mapper.Map<LoanDto>(value);
        }

        public async Task<List<LoanDto>> GetAsync()
        {
            var values = await _repository.GetAllListAsync();
            return _mapper.Map<List<LoanDto>>(values);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _repository.GetByIdAsync(id);
            _repository.Remove(investment);
            return Result.Ok().WithSuccess("Investimento deletado");
        }

        public async Task<ForecastList> GetForecast(EForecastType forecastType, DateTime maxYearMonth, DateTime currentDate)
        {
            var values = await GetAsync();

            var forecast = _forecast.GetForecast(values, forecastType, maxYearMonth, currentDate);

            return forecast;
        }
    }
}