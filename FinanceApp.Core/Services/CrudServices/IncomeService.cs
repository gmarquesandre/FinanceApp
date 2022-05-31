﻿using AutoMapper;
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
    public class IncomeService : CrudServiceBase, IIncomeService
    {

        public IncomeService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<Result> AddAsync(CreateIncome input, CustomIdentityUser user)
        {
            try
            {
                Income model = _mapper.Map<Income>(input);

                CheckValue(model);

                model.UserId = user.Id;
                await _context.Incomes.AddAsync(model);
                await _context.SaveChangesAsync();
                //return _mapper.Map<IncomeDto>(model);

                return Result.Ok().WithSuccess("Criado com sucesso");
            }
            catch (Exception ex)
            {
                return Result.Ok().WithError(ex.Message);
            }
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
    }
}