using AutoMapper;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class PrivateFixedIncomeService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        public PrivateFixedIncomeService(UserDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<PrivateFixedIncomeDto> AddInvestmentAsync(CreatePrivateFixedIncome input, CustomIdentityUser user)
        {
            PrivateFixedIncome model = _mapper.Map<PrivateFixedIncome>(input);

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            model.UserId = user.Id;
            await _context.PrivateFixedIncomes.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<PrivateFixedIncomeDto>(model);
            
        }
        public async Task<Result> UpdateInvestmentAsync(UpdatePrivateFixedIncome input, CustomIdentityUser user)
        {
            var oldModel = _context.PrivateFixedIncomes.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            else if (oldModel.UserId != user.Id)
                return Result.Fail("Usuário Inválido");

            var model = _mapper.Map<PrivateFixedIncome>(input);

            model.User = user;

            if (model.PreFixedInvestment && model.Index != EIndex.Prefixado)
                model.Index = EIndex.Prefixado;

            _context.PrivateFixedIncomes.Update(model);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }
        public async Task<List<PrivateFixedIncomeDto>> GetAllFixedIncomeAsync(CustomIdentityUser user)
        {
            var values = await _context.PrivateFixedIncomes.Where(a => a.User.Id == user.Id).ToListAsync();
            return _mapper.Map<List<PrivateFixedIncomeDto>>(values);
        }
        public async Task<Result> DeleteInvestmentAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == id);

            if(investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if(investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.PrivateFixedIncomes.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}