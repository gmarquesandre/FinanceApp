﻿using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices
{
    public class CurrentBalanceService : CrudServiceBase, ICurrentBalanceService
    {

        public CurrentBalanceService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input, CustomIdentityUser user)
        {
            var value = await _context.CurrentBalances.AsNoTracking().FirstOrDefaultAsync(a => a.User.Id == user.Id);
            CurrentBalance model = new();
            if (value == null)
            {
                model = _mapper.Map<CurrentBalance>(input);
                model.UserId = user.Id;
                model.CreationDateTime = DateTime.Now;
                await _context.CurrentBalances.AddAsync(model);
            }
            else
            {
                model = _mapper.Map<CurrentBalance>(input);
                model.Id = value.Id;
                model.UserId = user.Id;
                _context.CurrentBalances.Update(model);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<CurrentBalanceDto>(model);

        }

        public async Task<CurrentBalanceDto> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.CurrentBalances.FirstOrDefaultAsync(a => a.User.Id == user.Id);
            return _mapper.Map<CurrentBalanceDto>(values);
        }


        public async Task<Result> DeleteAsync(CustomIdentityUser user)
        {
            var investment = await _context.CurrentBalances.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (investment == null)
                throw new Exception("Não encotrado");

            _context.CurrentBalances.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}