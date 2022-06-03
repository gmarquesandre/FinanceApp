using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class IncomeService : CrudServiceBase, IIncomeService
    {
        public IncomeForecast _forecast;
        public IncomeService(FinanceContext context, IncomeForecast forecast, IMapper mapper) : base(context, mapper) 
        {
            _forecast = forecast;
        }

        public async Task<IncomeDto> AddAsync(CreateIncome input, CustomIdentityUser user)
        {

            Income model = _mapper.Map<Income>(input);

            CheckValue(model);

            model.UserId = user.Id;
            await _context.Incomes.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<IncomeDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateIncome input, CustomIdentityUser user)
        {
            var oldModel = _context.Incomes.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<Income>(input);

            CheckValue(model);

            model.User = user;
            model.CreationDateTime = oldModel.CreationDateTime;

            _context.Incomes.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }



        public async Task<List<IncomeDto>> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.Incomes.Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<IncomeDto>>(values);
        }

        public async Task<IncomeDto> GetAsync(CustomIdentityUser user, int id)
        {
            var value = await _context.Incomes.FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<IncomeDto>(value);

        }

        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.Incomes.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if (investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.Incomes.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }

        public async Task<List<ForecastItem>> GetForecast(CustomIdentityUser user)
        {
            var values = await GetAsync(user);

            var forecast = _forecast.GetMonthlyForecast(values, DateTime.Now.AddMonths(12));

            return forecast;
        }

        public void CheckValue(Income model)
        {
            if (model.Recurrence == ERecurrence.NTimes && (model.TimesRecurrence == null || model.TimesRecurrence == 0))
                throw new Exception("A quantidade de repetições deve ser maior que zero para o tipo de recorrência selecionado");

            else if (model.Recurrence != ERecurrence.NTimes && model.Recurrence != ERecurrence.Once && model.EndDate == null)
                throw new Exception("A data final deve ser preenchida");

            else if (model.Amount <= 0.00M)
                throw new Exception("O valor deve ser maior do que zero");

        }
    }
}