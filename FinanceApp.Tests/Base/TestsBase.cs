using AutoMapper;
using FinanceApp.Shared.Profiles;
using System.Globalization;

namespace FinanceApp.Tests.Base
{
    public class TestsBase
    {
        
        public readonly NumberFormatInfo SetPrecision = new()
        {
            NumberDecimalDigits = 2
        };
       

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
            var myProfile15 = new ForecastParametersProfile();


            var configuration = new MapperConfiguration(cfg =>
            {
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
                cfg.AddProfile(myProfile15);
            });

            IMapper mapper = new Mapper(configuration);

            return mapper;

        }
    }
}