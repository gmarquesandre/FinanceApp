﻿using AutoMapper;
using FinanceApp.Core.Services.Base;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Models;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.Core.Services
{
    public class FGTSService : CrudServiceBase, IFGTSService
    {

        public FGTSService(FinanceContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input, CustomIdentityUser user)
        {
            var value = await _context.FGTS.AsNoTracking().FirstOrDefaultAsync(a => a.User.Id == user.Id);
            FGTS model = new();
            if (value == null)
            {
                model = _mapper.Map<FGTS>(input);
                model.UserId = user.Id;
                model.CreationDateTime = DateTime.Now;
                await _context.FGTS.AddAsync(model);
            }
            else
            {
                model = _mapper.Map<FGTS>(input);
                model.Id = value.Id;
                model.UserId = user.Id;
                _context.FGTS.Update(model);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<FGTSDto>(model);

        }

        public async Task<FGTSDto> GetAsync(CustomIdentityUser user)
        {
            var values = await _context.FGTS.FirstOrDefaultAsync(a => a.User.Id == user.Id);
            return _mapper.Map<FGTSDto>(values);
        }


        public async Task<Result> DeleteAsync(CustomIdentityUser user)
        {
            var investment = await _context.FGTS.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (investment == null)
                throw new Exception("Não encotrado");

            _context.FGTS.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}