using AutoMapper;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class FixedInterestInvestmentService
    {
        private UserDbContext _context;
        private IMapper _mapper;
        public FixedInterestInvestmentService(UserDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<Result> AddInvestmentAsync(FixedInterestInvestmentDto input, CustomIdentityUser user)
        {
            FixedInterestInvestment model = _mapper.Map<FixedInterestInvestment>(input);

            model.UserId = user.Id;

            try {
                await _context.FixedInterestInvestments.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Ok().WithSuccess("Cadastrado com sucesso");
            }
            catch (Exception ex) {
                return Result.Fail($"Erro ao adicionar investimento");
            }
        }

        public async Task<Result> UpdateInvestmentAsync(FixedInterestInvestment input, CustomIdentityUser user)
        {

            //Permitir apenas o próprio usuário alterar o investimento
            if (user.Id != input.UserId)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.FixedInterestInvestments.Update(input);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento atualizado com sucesso");
        }

        public async Task<Result> DeleteInvestmentAsync(int id, CustomIdentityUser user)
        {
            var investment = await _context.FixedInterestInvestments.FirstOrDefaultAsync(a => a.Id == id);

            if(investment == null)
            {
                return Result.Fail("Não Encontrado");
            }

            if(investment.UserId != user.Id)
            {
                return Result.Fail("Usuário Inválido");
            }

            _context.FixedInterestInvestments.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}