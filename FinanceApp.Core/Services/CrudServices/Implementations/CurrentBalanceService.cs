using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class CurrentBalanceService : CrudServiceBase, ICurrentBalanceService
    {
        public CurrentBalanceService(FinanceContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context, mapper, httpContextAccessor) {}

        public async Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input)
        {
            var value = await _context.CurrentBalances.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId());
            
            
            
            CurrentBalance model = new();
            if (value == null)
            {
                model = _mapper.Map<CurrentBalance>(input);
                model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
                model.CreationDateTime = DateTime.Now;
                await _context.CurrentBalances.AddAsync(model);
            }
            else
            {
                model = _mapper.Map<CurrentBalance>(input);
                model.Id = value.Id;
                model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
                _context.CurrentBalances.Update(model);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<CurrentBalanceDto>(model);

        }

        public async Task<CurrentBalanceDto> GetAsync()
        {

            //string? a = _userContext!.HttpContext!.User!.FindFirst(ClaimTypes.NameIdentifier).Value;
            var value = await _context.CurrentBalances.FirstOrDefaultAsync();
            if (value != null)
            {
                return _mapper.Map<CurrentBalanceDto>(value);
            }
            else
            {
                return new CurrentBalanceDto()
                {
                    PercentageCdi = null,
                    UpdateValueWithCdiIndex = false,
                    Value = 0.00,
                    CreationDateTime = new DateTime(1900,1,1),
                    UpdateDateTime = new DateTime(1900, 1, 1),

                };
            }
        }


        public async Task<Result> DeleteAsync()
        {
            var investment = await _context.CurrentBalances.FirstOrDefaultAsync(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId());
            if (investment == null)
                throw new Exception("Não encotrado");

            _context.CurrentBalances.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}