
using AutoMapper;
using FinanceApp.Core.Services.Base;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class SpendingService : CrudServiceBase, ISpendingService
    {

        public SpendingService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<SpendingDto> AddAsync(CreateSpending input, CustomIdentityUser user)
        {
            Spending model = _mapper.Map<Spending>(input);
            CheckValue(model);
            model.UserId = user.Id;
            await _context.Spendings.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<SpendingDto>(model);

        }
        public async Task<Result> UpdateAsync(UpdateSpending input, CustomIdentityUser user)
        {
            var oldModel = _context.Spendings.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<Spending>(input);
            
            CheckValue(model);
            
            model.User = user;
            model.CreationDateTime = oldModel.CreationDateTime;

            _context.Spendings.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<List<SpendingDto>> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.Spendings.Include(a => a.Category).Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<SpendingDto>>(values);
        }

        public async Task<SpendingDto> GetAsync(CustomIdentityUser user, int id)
        {
            var value = await _context.Spendings.Include(a=> a.Category).FirstOrDefaultAsync(a => a.User.Id == user.Id && a.Id == id);

            if (value == null)
                throw new Exception("Registro Não Encontrado");

            return _mapper.Map<SpendingDto>(value);

        }

        public async Task<Result> DeleteAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.Spendings.FirstOrDefaultAsync(a => a.Id == id);

            if (investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if (investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.Spendings.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}