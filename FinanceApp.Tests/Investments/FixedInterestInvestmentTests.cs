using AutoMapper;
using FinanceApp.Core.Services;
using FinanceApp.EntityFramework.Auth;
using FinanceApp.Shared.Dto;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsuariosApi.Models;
using UsuariosApi.Profiles;
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

        private PrivateFixedIncomeService GetPrivateFixedIncomeService(UserDbContext userContext)
        {
            IMapper mapper = GetPrivateFixedIncomeServiceMapper();
            var service = new PrivateFixedIncomeService(userContext, mapper);

            return service;
        }
        private static IMapper GetPrivateFixedIncomeServiceMapper()
        {
            var myProfile = new PrivateFixedIncomeProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            return mapper;
        }

        [Fact]
        public async Task<(UserDbContext userContext, CustomIdentityUser user, PrivateFixedIncome investment)> MustAddInvestment()
        {
            var userContext = await CreateUserDbContext();

            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            PrivateFixedIncomeService service = GetPrivateFixedIncomeService(userContext);

            await service.AddInvestmentAsync(investment, user);

            var investments = await userContext.PrivateFixedIncomes.ToListAsync();

            Assert.True(investments != null);

            return (userContext, user, investments!.First());
        }

        [Fact]
        private async Task MustFailAddInvestment()
        {
            
            var userContext = await CreateUserDbContext();

            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            investment.Type = 45;

            PrivateFixedIncomeService service = GetPrivateFixedIncomeService(userContext);

            await service.AddInvestmentAsync(investment, user);

            var investments = await userContext.PrivateFixedIncomes.ToListAsync();

            Assert.True(investments != null);

            
        }

        [Fact]
        public async Task MustUpdateInvestment()
        {
            (UserDbContext userContext, CustomIdentityUser user, PrivateFixedIncome investment) = await MustAddInvestment();

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

            await service.UpdateInvestmentAsync(updateInvestment, user!);

            var newInvestment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == updateInvestment.Id);

            Assert.True(newInvestment!.Amount == investment.Amount*2);
            Assert.True(newInvestment!.ExpirationDate == investment.ExpirationDate.AddDays(10));
        }

        [Fact]
        public async Task MustDeleteInvestment()
        {
            (UserDbContext userContext, CustomIdentityUser user, PrivateFixedIncome investment) = await MustAddInvestment();
            
            var service = GetPrivateFixedIncomeService(userContext);

            var mustExistInvesment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustExistInvesment != null);

            await service.DeleteInvestmentAsync(investment.Id, user!);

            var mustBeNullInvestment = await userContext.PrivateFixedIncomes.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustBeNullInvestment == null);
        }
    }
}
