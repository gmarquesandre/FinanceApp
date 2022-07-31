//using AutoMapper;
//using FinanceApp.Core.Services;
//using FinanceApp.EntityFramework.Auth;
//using FinanceApp.Shared.Dto;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Shared.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using UsuariosApi.Models;
//using UsuariosApi.Profiles;
//using Xunit;

//namespace FinanceApp.Tests.Investments
//{
//    public class SpendingTests : AuthenticateTests
//    {
//        private readonly CreateSpending DefaultNewInvestment = new()
//        {
//            AdditionalFixedInterest = 0.00M,
//            Amount = 1000.00M,
//            ExpirationDate = DateTime.Now.Date.AddDays(90),
//            Index = (int)EIndex.CDI,
//            IndexPercentage = 100.00M,
//            InvestmentDate = DateTime.Now.Date,
//            LiquidityOnExpiration = true,
//            Name = "Teste",
//            PreFixedInvestment = false,
//            Type = (int)ETypeSpending.CRA
//        };

//        private SpendingService GetSpendingService(UserContext userContext)
//        {

//            var mapper = GetConfigurationIMapper();
//            var service = new SpendingService(userContext, mapper);

//            return service;
//        }

//        [Fact]
//        public async Task<(UserContext userContext, Spending investment)> MustAddInvestment()
//        {
//            var userContext = await CreateUserContext();

//            var user = await ReturnDefaultUser(userContext);

//            var investment = DefaultNewInvestment;

//            SpendingService service = GetSpendingService(userContext);

//            await service.AddAsync(investment);

//            var investments = await userContext.Spendings.AsNoTracking().ToListAsync();

//            Assert.True(investments != null);

//            return (userContext, user, investments!.First());
//        }

//        [Fact]
//        private async Task MustFailAddInvestment()
//        {

//            var userContext = await CreateUserContext();

//            var user = await ReturnDefaultUser(userContext);

//            var investment = DefaultNewInvestment;

//            investment.Type = 45;

//            SpendingService service = GetSpendingService(userContext);

//            await service.AddAsync(investment);

//            var investments = await userContext.Spendings.AsNoTracking().ToListAsync();

//            Assert.True(investments != null);


//        }

//        [Fact]
//        public async Task MustUpdateInvestment()
//        {
//            (UserContext userContext, Spending investment) = await MustAddInvestment();

//            var updateInvestment = new UpdateSpending()
//            {
//                Id = investment.Id,
//                Name = investment.Name,
//                AdditionalFixedInterest = investment.AdditionalFixedInterest,
//                Amount = investment.Amount * 2,
//                ExpirationDate = investment.ExpirationDate.AddDays(10),
//                IndexPercentage = investment.IndexPercentage,
//                Index = (int)investment.Index,
//                InvestmentDate = investment.InvestmentDate,
//                LiquidityOnExpiration = investment.LiquidityOnExpiration,
//                PreFixedInvestment = investment.PreFixedInvestment,
//                Type = (int)investment.Type
//            };


//            var service = GetSpendingService(userContext);

//            await service.UpdateAsync(updateInvestment, user!);

//            var newInvestment = await userContext.Spendings.FirstOrDefaultAsync(a => a.Id == updateInvestment.Id);

//            Assert.True(newInvestment!.Amount == investment.Amount * 2);
//            Assert.True(newInvestment!.ExpirationDate == investment.ExpirationDate.AddDays(10));
//        }

//        [Fact]
//        public async Task MustDeleteInvestment()
//        {
//            (UserContext userContext, Spending investment) = await MustAddInvestment();

//            var service = GetSpendingService(userContext);

//            var mustExistInvesment = await userContext.Spendings.FirstOrDefaultAsync(a => a.Id == investment.Id);

//            Assert.True(mustExistInvesment != null);

//            await service.DeleteAsync(investment.Id, user!);

//            var mustBeNullInvestment = await userContext.Spendings.FirstOrDefaultAsync(a => a.Id == investment.Id);

//            Assert.True(mustBeNullInvestment == null);
//        }
//    }
//}
