using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class LoanService : CrudServiceBase, ILoanService
    {
        public ILoanForecast _forecast;
        public LoanService(FinanceContext context, IMapper mapper, ILoanForecast forecast, IHttpContextAccessor httpContextAccessor) : base(context, mapper, httpContextAccessor) {
            _forecast = forecast;            
        }

        public async Task<LoanDto> AddAsync(CreateLoan input)
        {
            Loan model = _mapper.Map<Loan>(input);

            model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            await _context.Loans.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<LoanDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateLoan input)
        {
            var oldModel = _context.Loans.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != _httpContextAccessor.HttpContext.User.GetUserId())
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<Loan>(input);
            model.CreationDateTime = oldModel.CreationDateTime;
            model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();            

            _context.Loans.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<LoanDto> GetAsync(int id)
        {
            var value = await _context.Loans.FirstOrDefaultAsync();

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<LoanDto>(value);

        }

        public async Task<List<LoanDto>> GetAsync()
        {
            var values = await _context.Loans.ToListAsync();
            return _mapper.Map<List<LoanDto>>(values);
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var investment = await _context.Loans.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            _context.Loans.Remove(investment);
            await _context.SaveChangesAsync();
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