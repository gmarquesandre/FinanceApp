using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanceApp.EntityFramework;
using AutoMapper;
using System;
using System.Linq;
using System.Reflection;
using FinanceApp.Shared.Profiles;

namespace FinanceApp.Tests.Base
{
    public class TestsBase
    {

        public async Task<FinanceContext> CreateFinanceContext()
        {
            var options = new DbContextOptionsBuilder<FinanceContext>()
               .UseInMemoryDatabase("FinanceContext")
               .Options;

            var context = new FinanceContext(options);


            await context.Database.EnsureCreatedAsync();

            return context;
        }

        public async Task DeleteDataDb(FinanceContext context)
        {
            await context.Database.EnsureDeletedAsync();
        }


        public static IMapper GetConfigurationIMapper()
        {

            var myProfile = new CategoryProfile();
            var myProfile2 = new CreditCardProfile();
            var myProfile3 = new CurrentBalanceProfile();
            var myProfile4 = new FGTSProfile();
            var myProfile5 = new HolidayProfile();
            var myProfile6 = new IncomeProfile();
            var myProfile7 = new IndexValueProfile();
            var myProfile8 = new LoanProfile();
            var myProfile9 = new PrivateFixedIncomeProfile();
            var myProfile10 = new ProspectIndexValueProfile();
            var myProfile11 = new SpendingProfile();
            var myProfile12 = new TreasuryBondProfile();
            var myProfile13 = new UsuarioProfile();
            var myProfile14 = new WorkingDaysByYearProfile();


            var configuration = new MapperConfiguration(cfg => { 
                cfg.AddProfile(myProfile); 
                cfg.AddProfile(myProfile2);
                cfg.AddProfile(myProfile3);
                cfg.AddProfile(myProfile4);
                cfg.AddProfile(myProfile5);
                cfg.AddProfile(myProfile6);
                cfg.AddProfile(myProfile7);
                cfg.AddProfile(myProfile8);
                cfg.AddProfile(myProfile9);
                cfg.AddProfile(myProfile10);
                cfg.AddProfile(myProfile11);
                cfg.AddProfile(myProfile12);
                cfg.AddProfile(myProfile13);
                cfg.AddProfile(myProfile14);                       
            });

            IMapper mapper = new Mapper(configuration);

            return mapper;

        }
    }
}