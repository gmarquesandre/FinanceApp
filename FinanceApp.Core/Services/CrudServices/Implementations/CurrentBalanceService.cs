using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.Shared.Dto.CurrentBalance;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FinanceApp.EntityFramework;
using FinanceApp.Shared;

namespace FinanceApp.Core.Services.CrudServices.Implementations
{
    public class CurrentBalanceService : CrudServiceBase, ICurrentBalanceService
    {
        public IRepository<CurrentBalance> _repository;
        public CurrentBalanceService(FinanceContext context, IMapper mapper, IRepository<CurrentBalance> repository, IHttpContextAccessor httpContextAccessor) : base(context, mapper, httpContextAccessor) {
            _repository = repository;
        }

        public async Task<CurrentBalanceDto> AddOrUpdateAsync(CreateOrUpdateCurrentBalance input)
        {
            var value = await _context.CurrentBalances.AsNoTracking().FirstOrDefaultAsync();
            
            try { 
                CurrentBalance model = _mapper.Map<CurrentBalance>(input);
                if (value == null)
                {
                    await _repository.InsertAsync(model);
                }
                else
                {
                    model.Id = value.Id;
                    _repository.Update(model);
                }
                await _context.SaveChangesAsync();
                return _mapper.Map<CurrentBalanceDto>(model);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
                    Id = 0,
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