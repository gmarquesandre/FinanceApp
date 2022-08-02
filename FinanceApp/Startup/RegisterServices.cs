using FinanceApp.Api.Controllers;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Implementations;
using FinanceApp.Core.Services.CrudServices.CrudDefault.Interfaces;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Implementations;
using FinanceApp.Core.Services.CrudServices.CrudSingleRegister.Interfaces;
using FinanceApp.Core.Services.ForecastServices;
using FinanceApp.Core.Services.ForecastServices.Implementations;
using FinanceApp.Core.Services.ForecastServices.Interfaces;
using FinanceApp.Core.Services.UserServices;
using FinanceApp.Core.Services.UserServices.Interfaces;
using FinanceApp.EntityFramework.User;
using FinanceApp.FinanceData;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared;
using FinanceApp.Shared.Dto.Income;
using FinanceApp.Shared.Dto.Loan;
using FinanceApp.Shared.Dto.Spending;

namespace FinanceApp.Api.Startup
{
    public static class ServiceExtensions
    {

        //https://dev.to/tomfletcher9/net-6-register-services-using-reflection-3156
        public static void RegisterServices(this IServiceCollection services)
        {

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));


            services.AddScoped<ITokenService, TokenService>();

            //CRUD
            services.AddScoped<ILoginService, LoginService>();
          
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICurrentBalanceService, CurrentBalanceService>();
            services.AddScoped<IFGTSService, FGTSService>();
            services.AddScoped<IForecastParametersService, ForecastParametersService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<ILoanService, LoanService>();
            services.AddScoped<IPrivateFixedIncomeService, PrivateFixedIncomeService>();
            services.AddScoped<ISpendingService, SpendingService>();
            //services.AddScoped<ITreasuryBondService, TreasuryBondService>();


            //Common
            services.AddScoped<IDatesService, DatesService>();
            services.AddScoped<IIndexService, IndexService>();
            services.AddScoped<ITitleService, TitleService>();

            //Forecast
            services.AddScoped<IForecastService, ForecastService>();
            services.AddScoped<ILoanForecast, LoanForecast>();
            services.AddScoped<ISpendingForecast, SpendingForecast>();
            services.AddScoped<IForecastParametersService, ForecastParametersService>();
            services.AddScoped<IIncomeForecast, IncomeForecast>();



        }
    }
}
