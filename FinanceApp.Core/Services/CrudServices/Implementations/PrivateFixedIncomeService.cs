using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class PrivateFixedIncomeService : CrudServiceBase, IPrivateFixedIncomeService
    {

        public PrivateFixedIncomeService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<PrivateFixedIncomeDto> AddAsync(CreatePrivateFixedIncome input, CustomIdentityUser user)
        {
            PrivateFixedIncome model = _mapper.Map<PrivateFixedIncome>(input);

            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            model.UserId = user.Id;
            await _context.PrivateFixedIncomes.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<PrivateFixedIncomeDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdatePrivateFixedIncome input, CustomIdentityUser user)
        {
            var oldModel = _context.PrivateFixedIncomes.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<PrivateFixedIncome>(input);
            model.CreationDateTime = oldModel.CreationDateTime;
            model.User = user;


            CheckInvestment(model);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            _context.PrivateFixedIncomes.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<PrivateFixedIncomeDto> GetAsync(CustomIdentityUser user, int id)
        {
            var value = await _context.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<PrivateFixedIncomeDto>(value);

        }

        private void CheckInvestment(PrivateFixedIncome model)
        {
            if (model.InvestmentDate > DateTime.Now.Date)
            {
                throw new Exception("A data de investimento não pode ser maior do que hoje");
            }
            else if (model.InvestmentDate >= model.ExpirationDate)
            {
                throw new Exception("A data de vencimento deve ser maior do que a de investimento ");
            }
            else if (model.ExpirationDate <= DateTime.Now.Date)
            {
                throw new Exception("A data de vencimento deve ser maior do que hoje");
            }


        }

        public async Task<List<PrivateFixedIncomeDto>> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.PrivateFixedIncomes.Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<PrivateFixedIncomeDto>>(values);
        }
        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if (investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.PrivateFixedIncomes.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}