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
    public class TreasuryBondTests : AuthenticateTests
    {
        private readonly CreateIncome DefaultNewInvestment = new()
        {
            ExpirationDate = new DateTime(2023,01,01),
            InvestmentDate = DateTime.Now.Date,
            Type = (int)ETreasuryBond.LTN,
            Operation = (int)EOperation.Buy,
            Quantity = 1,
            UnitPrice = 50

        };

        private TreasuryBondService GetTreasuryBondService(FinanceContext userContext)
        {
            IMapper mapper = GetTreasuryBondServiceMapper();
            var service = new TreasuryBondService(userContext, mapper);

            return service;
        }
        private static IMapper GetTreasuryBondServiceMapper()
        {
            var myProfile = new TreasuryBondProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            return mapper;
        }

        [Fact]
        public async Task<(FinanceContext userContext, CustomIdentityUser user, TreasuryBond investment)> MustAddInvestment()
        {
            var userContext = await CreateFinanceContext();

            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            TreasuryBondService service = GetTreasuryBondService(userContext);

            await service.AddAsync(investment, user);

            var investments = await userContext.TreasuryBonds.AsNoTracking().ToListAsync();

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

            TreasuryBondService service = GetTreasuryBondService(userContext);

            await service.AddAsync(investment, user);

            var investments = await userContext.TreasuryBonds.AsNoTracking().ToListAsync();

            Assert.True(investments != null);

            
        }

        [Fact]
        public async Task MustUpdateInvestment()
        {
            (FinanceContext userContext, CustomIdentityUser user, TreasuryBond investment) = await MustAddInvestment();

            var updateInvestment = new UpdateIncome()
            {
                ExpirationDate = investment.ExpirationDate,
                InvestmentDate = investment.InvestmentDate,
                Type = (int)investment.Type,
                Operation = (int)investment.Operation,
                Quantity = investment.Quantity*2,
                UnitPrice = 50,
                Id = investment.Id
            };
                

            var service = GetTreasuryBondService(userContext);

            await service.UpdateAsync(updateInvestment, user!);

            var newInvestment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == updateInvestment.Id);

            Assert.True(newInvestment!.Quantity == investment.Quantity * 2);
            Assert.True(newInvestment!.ExpirationDate == investment.ExpirationDate);
        }

        [Fact]
        public async Task MustDeleteInvestment()
        {
            (FinanceContext userContext, CustomIdentityUser user, TreasuryBond investment) = await MustAddInvestment();
            
            var service = GetTreasuryBondService(userContext);

            var mustExistInvesment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustExistInvesment != null);

            await service.DeleteAsync(investment.Id, user!);

            var mustBeNullInvestment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustBeNullInvestment == null);
        }
    }
}
