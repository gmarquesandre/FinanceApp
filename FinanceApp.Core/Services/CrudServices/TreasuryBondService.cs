using AutoMapper;
using FinanceApp.Core.Services.Base;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class TreasuryBondService : CrudServiceBase, ITreasuryBondService
    {

        public TreasuryBondService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<TreasuryBondDto> AddAsync(CreateTreasuryBond input, CustomIdentityUser user)
        {
            TreasuryBond model = _mapper.Map<TreasuryBond>(input);

            CheckInvestment(model);

            model.UserId = user.Id;
            await _context.TreasuryBonds.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<TreasuryBondDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateTreasuryBond input, CustomIdentityUser user)
        {
            var oldModel = _context.TreasuryBonds.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<TreasuryBond>(input);

            model.User = user;
            model.CreationDateTime = oldModel.CreationDateTime;

            CheckInvestment(model);

            _context.TreasuryBonds.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        private void CheckInvestment(TreasuryBond model)
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
            else if (model.Operation != EOperation.Buy && model.Operation != EOperation.Sell)
                throw new Exception("Operação Inválida");


        }

        public async Task<List<TreasuryBondDto>> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.TreasuryBonds.Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<TreasuryBondDto>>(values);
        }

        public async Task<TreasuryBondDto> GetAsync(CustomIdentityUser user, int id)
        {
            var value = await _context.TreasuryBonds.FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<TreasuryBondDto>(value);

        }

        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if (investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.TreasuryBonds.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}