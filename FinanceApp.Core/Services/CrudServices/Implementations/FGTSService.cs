using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.FGTS;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class FGTSService : CrudServiceBase, IFGTSService
    {

        public FGTSService(FinanceContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context, mapper, httpContextAccessor) { }

        public async Task<FGTSDto> AddOrUpdateAsync(CreateOrUpdateFGTS input)
        {
            var value = await _context.FGTS.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId());
            FGTS model = new();
            if (value == null)
            {
                model = _mapper.Map<FGTS>(input);
                model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
                model.CreationDateTime = DateTime.Now;
                await _context.FGTS.AddAsync(model);
            }
            else
            {
                model = _mapper.Map<FGTS>(input);
                model.Id = value.Id;
                model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
                _context.FGTS.Update(model);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<FGTSDto>(model);

        }

        public async Task<FGTSDto> GetAsync()
        {
            var value = await _context.FGTS.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<FGTSDto>(value);
            }
            else
            {
                return new FGTSDto()
                {
                    AnniversaryWithdraw = false,
                    CurrentBalance = 0.00,
                    MonthlyGrossIncome = 0.00                     
                };
            }
        }


        public async Task<Result> DeleteAsync()
        {
            var investment = await _context.FGTS.FirstOrDefaultAsync();
            if (investment == null)
                throw new Exception("Não encotrado");

            _context.FGTS.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}