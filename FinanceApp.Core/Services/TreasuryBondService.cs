using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Base;
using FinanceApp.Shared.Dto.TreasuryBond;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FinanceApp.Shared;
using FinanceApp.EntityFramework;

namespace FinanceApp.Core.Services
{
    public class TreasuryBondService : CrudServiceBase
    {
        public TreasuryBondService(FinanceContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(context, mapper, httpContextAccessor)
        {
        } 

        public async Task<TreasuryBondDto> AddInvestmentAsync(CreateTreasuryBond input)
        {
            TreasuryBond model = _mapper.Map<TreasuryBond>(input);
            if (model.Operation != EOperation.Buy && model.Operation != EOperation.Sell)
                throw new Exception("Operação Inválida");
            CheckInvestment(model);

            model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            await _context.TreasuryBonds.AddAsync(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<TreasuryBondDto>(model);
            
        }
        public async Task<Result> UpdateInvestmentAsync(UpdateTreasuryBond input)
        {
            var oldModel = _context.TreasuryBonds.AsNoTracking().FirstOrDefault(x => x.Id == input.Id);

            if (oldModel == null)
                return Result.Fail("Já foi deletado");
            
            var model = _mapper.Map<TreasuryBond>(input);

            model.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
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
            

        }

        public async Task<List<TreasuryBondDto>> GetAllFixedIncomeAsync()
        {
            var values = await _context.TreasuryBonds.ToListAsync();
            return _mapper.Map<List<TreasuryBondDto>>(values);
        }
        public async Task<Result> DeleteInvestmentAsync(int id)
        {
            var investment = await _context.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == id);

            if(investment == null)
            {
                return Result.Fail("Não Encontrado");
            }            

            _context.TreasuryBonds.Remove(investment);
            await _context.SaveChangesAsync();
            return Result.Ok().WithSuccess("Investimento deletado");
        }
    }
}