using FinanceApp.EntityFramework.Auth;
using FluentResults;

namespace FinanceApp.Core.Services
{
    public class FixedInterestInvestmentService
    {
        private UserDbContext _context;

        public FixedInterestInvestmentService(UserDbContext context) 
        {
            _context = context;
        }

        public async Task<Result> AddInvestmentAsync(FixedInterestInvestment input)
        {
            try {
                await _context.FixedInterestInvestments.AddAsync(input);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex) {
                return Result.Fail($"Erro ao adicionar investimento");
            }
        }

    }
}