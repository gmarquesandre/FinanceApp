using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Implementations;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.PrivateFixedInvestment;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using FinanceApp.Shared.Models.UserTables;
using FinanceApp.Shared.Profiles;
using FinanceApp.Tests.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Investments
{
    public class PrivateFixedIncomeTests : AuthenticateTests
    {
        private readonly CreatePrivateFixedIncome DefaultNewInvestment = new()
        {
            AdditionalFixedInterest = 0.00M,
            Amount = 1000.00M,
            ExpirationDate = DateTime.Now.Date.AddDays(90),
            Index = (int)EIndex.CDI,
            IndexPercentage = 100.00M,
            InvestmentDate = DateTime.Now.Date,
            LiquidityOnExpiration = true,
            Name = "Teste",
            PreFixedInvestment = false,
            Type = (int)ETypePrivateFixedIncome.CRA
        };

        private PrivateFixedIncomeService GetPrivateFixedIncomeService(FinanceContext userContext)
        {
            var mapper = GetConfigurationIMapper();
            var service = new PrivateFixedIncomeService(userContext, mapper);

            return service;
        }
       
        [Fact]
        public async Task<(FinanceContext userContext, CustomIdentityUser user, PrivateFixedIncome investment)> MustAddInvestment()
        {
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            PrivateFixedIncomeService service = GetPrivateFixedIncomeService(userContext);

            await service.AddAsync(investment, user);

            var investments = await userContext.PrivateFixedIncomes.AsNoTracking().ToListAsync();

            Assert.True(investments != null);

            return (userContext, user, investments!.First());
        }

        [Fact]
        private async Task MustFailAddInvestment()
        {
            
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            investment.Type = 45;

            PrivateFixedIncomeService service = GetPrivateFixedIncomeService(userContext);

            await service.AddAsync(investment, user);

            var investments = await userContext.PrivateFixedIncomes.AsNoTracking().ToListAsync();

            Assert.True(investments != null);

            
        }

        [Fact]
        public async Task MustUpdateInvestment()
        {
            (FinanceContext userContext, CustomIdentityUser user, PrivateFixedIncome investment) = await MustAddInvestment();

            var updateInvestment = new UpdatePrivateFixedIncome()
            {
                Id = investment.Id,
                Name = investment.Name,
                AdditionalFixedInterest = investment.AdditionalFixedInterest,
                Amount = investment.Amount * 2,
                ExpirationDate = investment.ExpirationDate.AddDays(10),
                IndexPercentage = investment.IndexPercentage,
                Index = (int)investment.Index,
                InvestmentDate = investment.InvestmentDate,
                LiquidityOnExpiration = investment.LiquidityOnExpiration,
                PreFixedInvestment = investment.PreFixedInvestment,
                Type = (int)investment.Type
            };
                

            var service = GetPrivateFixedIncomeService(userContext);

            await service.UpdateAsync(updateInvestment, user!);

            var newInvestment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == updateInvestment.Id);

            Assert.True(newInvestment!.Amount == investment.Amount*2);
            Assert.True(newInvestment!.ExpirationDate == investment.ExpirationDate.AddDays(10));
        }

        [Fact]
        public async Task MustDeleteInvestment()
        {
            (FinanceContext userContext, CustomIdentityUser user, PrivateFixedIncome investment) = await MustAddInvestment();
            
            var service = GetPrivateFixedIncomeService(userContext);

            var mustExistInvesment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustExistInvesment != null);

            await service.DeleteAsync(investment.Id, user!);

            var mustBeNullInvestment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustBeNullInvestment == null);
        }
    }
}
