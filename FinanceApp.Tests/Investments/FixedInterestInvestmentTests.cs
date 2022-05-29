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
using UsuariosApi.Profiles;
using Xunit;

namespace FinanceApp.Tests.Investments
{
    public class FixedInterestInvestmentTests : AuthenticateTests
    {
        private FixedInterestInvestmentDto DefaultNewInvestment = new()
        {
            AdditionalFixedInterest = 0.00M,
            Amount = 1000.00M,
            ExpirationDate = DateTime.Now.Date.AddDays(90),
            Index = EIndex.CDI,
            IndexPercentage = 100.00M,
            InvestmentDate = DateTime.Now.Date,
            LiquidityOnExpiration = true,
            Name = "Teste",
            PreFixedInvestment = false,
            Type = ETypeFixedInterestInvestment.CDB
        };

        private FixedInterestInvestmentService GetFixedInterestInvestmentService(UserDbContext userContext)
        {
            IMapper mapper = GetFixedInterestInvestmentServiceMapper();
            var service = new FixedInterestInvestmentService(userContext, mapper);

            return service;
        }
        private IMapper GetFixedInterestInvestmentServiceMapper()
        {
            var myProfile = new FixedInterestInvestmentProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            return mapper;
        }

        [Fact]
        public async Task<(UserDbContext userContext, FixedInterestInvestment investment)> MustAddInvestment()
        {
            var userContext = await CreateUserDbContext();
           
            var user = await ReturnDefaultUser(userContext);

            var investment = DefaultNewInvestment;

            FixedInterestInvestmentService service = GetFixedInterestInvestmentService(userContext);

            await service.AddInvestmentAsync(investment, user);

            var investments = await userContext.FixedInterestInvestments.ToListAsync();

            Assert.True(investments != null);

            return (userContext, investments!.First());
        }

        [Fact]
        public async Task MustUpdateInvestment()
        {
            (UserDbContext userContext, FixedInterestInvestment investment ) = await MustAddInvestment();

            var updateInvestment = investment;

            var user = await userContext.Users.FirstOrDefaultAsync(a => a.Id == updateInvestment.UserId);

            updateInvestment.Amount = 2000.00M;

            var service = GetFixedInterestInvestmentService(userContext);

            await service.UpdateInvestmentAsync(updateInvestment, user!);

            var newInvestment = await userContext.FixedInterestInvestments.FirstOrDefaultAsync(a=> a.Id == updateInvestment.Id);

            Assert.True(newInvestment.Amount == 2000.00M);
        }        

        [Fact]
        public async Task MustDeleteInvestment()
        {
            (UserDbContext userContext, FixedInterestInvestment investment) = await MustAddInvestment();

            var user = await userContext.Users.FirstOrDefaultAsync(a => a.Id == investment.UserId);

            var service = GetFixedInterestInvestmentService(userContext);

            var mustExistInvesment = await userContext.FixedInterestInvestments.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustExistInvesment != null);

            await service.DeleteInvestmentAsync(investment.Id, user!);

            var mustBeNullInvestment = await userContext.FixedInterestInvestments.FirstOrDefaultAsync(a => a.Id == investment.Id);

            Assert.True(mustBeNullInvestment == null);
        }
    }
}
