//using AutoMapper;
//using FinanceApp.Core.Services.CrudServices.Implementations;
//using FinanceApp.EntityFramework;
//using FinanceApp.Shared.Dto.TreasuryBond;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Shared.Models.CommonTables;
//using FinanceApp.Shared.Models.UserTables;
//using FinanceApp.Shared.Profiles;
//using FinanceApp.Tests.Base;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace FinanceApp.Tests.Investments
//{
//    public class TreasuryBondTests : AuthenticateTests
//    {
//        private readonly CreateTreasuryBond DefaultNewInvestment = new()
//        {
//            ExpirationDate = new DateTime(2023,01,01),
//            InvestmentDate = DateTime.Now.Date,
//            Type = (int)ETreasuryBond.LTN,
//            Operation = (int)EOperation.Buy,
//            Quantity = 1,
//            UnitPrice = 50

//        };

//        private TreasuryBondService GetTreasuryBondService(UserContext userContext)
//        {

//            var mapper = GetConfigurationIMapper(); var service = new TreasuryBondService(userContext, mapper);

//            return service;
//        }
        
//        [Fact]
//        public async Task<(UserContext userContext, TreasuryBond investment)> MustAddInvestment()
//        {
//            var userContext = await CreateUserContext();

//            var user = await ReturnDefaultUser(userContext);

//            var investment = DefaultNewInvestment;

//            TreasuryBondService service = GetTreasuryBondService(userContext);

//            await service.AddAsync(investment);

//            var investments = await userContext.TreasuryBonds.AsNoTracking().ToListAsync();

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

//            TreasuryBondService service = GetTreasuryBondService(userContext);

//            await service.AddAsync(investment);

//            var investments = await userContext.TreasuryBonds.AsNoTracking().ToListAsync();

//            Assert.True(investments != null);

            
//        }

//        [Fact]
//        public async Task MustUpdateInvestment()
//        {
//            (UserContext userContext, TreasuryBond investment) = await MustAddInvestment();

//            var updateInvestment = new UpdateTreasuryBond()
//            {
//                ExpirationDate = investment.ExpirationDate,
//                InvestmentDate = investment.InvestmentDate,
//                Type = (int)investment.Type,
//                Operation = (int)investment.Operation,
//                Quantity = investment.Quantity*2,
//                UnitPrice = 50,
//                Id = investment.Id
//            };
                

//            var service = GetTreasuryBondService(userContext);

//            await service.UpdateAsync(updateInvestment);

//            var newInvestment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == updateInvestment.Id);

//            Assert.True(newInvestment!.Quantity == investment.Quantity * 2);
//            Assert.True(newInvestment!.ExpirationDate == investment.ExpirationDate);
//        }

//        [Fact]
//        public async Task MustDeleteInvestment()
//        {
//            (UserContext userContext, TreasuryBond investment) = await MustAddInvestment();
            
//            var service = GetTreasuryBondService(userContext);

//            var mustExistInvesment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == investment.Id);

//            Assert.True(mustExistInvesment != null);

//            await service.DeleteAsync(investment.Id);

//            var mustBeNullInvestment = await userContext.TreasuryBonds.FirstOrDefaultAsync(a => a.Id == investment.Id);

//            Assert.True(mustBeNullInvestment == null);
//        }
//    }
//}
