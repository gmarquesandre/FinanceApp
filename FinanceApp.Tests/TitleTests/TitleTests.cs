using FinanceApp.FinanceData;
using FinanceApp.FinanceData.Importers;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.TitleTests
{
    public class ForecastTests : AuthenticateTests
    {

        public async Task<TitleService> TitleServiceInstanceAsync()
        {

            var context = await CreateFinanceDataContext();
            var mapper = GetConfigurationIMapper();
            
            //Instancias 
            MemoryCacheOptions cacheOptions = new();

            var memoryCache = new MemoryCache(cacheOptions);

            var datesService = new DatesService(context,
                                                mapper,
                                                memoryCache);


            var indexService = new IndexService(context,
                                                mapper,
                                                memoryCache,
                                                datesService);

            var titleService = new TitleService(indexService);
            return titleService;
        }

        [Fact]
        public async Task MustReturnCorrectCurrentBalanceValueAsync()
        {

            var context = await CreateFinanceDataContext();
            var contextData = await CreateFinanceDataContext();
            //GetCurrentValueOfTitle(DateTime dateInvestment, double investmentValue, DateTime date, bool updateWithCdiIndex, double indexPercentage)
            var titleService = await TitleServiceInstanceAsync();
            var indexesImporter = new IndexImporter(contextData);

            var holidaysImporter = new HolidaysImporter(contextData);

            //Importar dados para comparação

            await holidaysImporter.GetHolidays(2022, 2022);

            await indexesImporter.GetIndexes(indexImport: EIndex.CDI, dateStart: new DateTime(2022, 01, 01));


            var input = new DefaultTitleInput()
            {
                DateInvestment = new DateTime(2022, 05, 31),
                Date = new DateTime(2022, 06, 20),
                AdditionalFixedInterest = 0,
                Index = EIndex.CDI,
                IndexPercentage = 1.03,
                InvestmentValue = 395.65,
                TypePrivateFixedIncome = ETypePrivateFixedIncome.CDB
            };

            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5


            var value = await titleService.GetCurrentValueOfTitle(input);

            Assert.True(value.GrossValue.ToString("N", SetPrecision) == 398.17.ToString("N", SetPrecision));
            Assert.True(value.LiquidValue.ToString("N", SetPrecision) == 396.96.ToString("N", SetPrecision));
            Assert.True(value.IofValue.ToString("N", SetPrecision) == 0.83.ToString("N", SetPrecision));
            Assert.True(value.IncomeTaxValue.ToString("N", SetPrecision) == 0.37.ToString("N", SetPrecision));


            //-----------------------

            input.InvestmentValue = 345.82;
            input.DateInvestment = new DateTime(2022, 05, 31);

            value = await titleService.GetCurrentValueOfTitle(input);

            Assert.True(value.GrossValue.ToString("N", SetPrecision) == 348.02.ToString("N", SetPrecision));
            Assert.True(value.IofValue.ToString("N", SetPrecision) == 0.72.ToString("N", SetPrecision));
            Assert.True(value.LiquidValue.ToString("N", SetPrecision) == 346.97.ToString("N", SetPrecision));
            Assert.True(value.IncomeTaxValue.ToString("N", SetPrecision) == 0.33.ToString("N", SetPrecision));


            //-----------------------

            input.InvestmentValue = 633.11;
            input.DateInvestment = new DateTime(2022, 04, 11);
            value = await titleService.GetCurrentValueOfTitle(input);

            Assert.True(value.GrossValue.ToString("N", SetPrecision) == 647.40.ToString("N", SetPrecision));

        }
        public static readonly object[][] TestWithdrawCenario =
        {
            new object[] { 1074.93, new DateTime(2022, 04, 29), new DateTime(2022, 06, 06), 1.03, 1083.45, 3.03, 1080.42, 1069.98, 5},
        };

        [Theory, MemberData(nameof(TestWithdrawCenario))]
        public async Task MustReturnTitleValueAfterWithdraw(double investmentValue, 
            DateTime dateInvestment, 
            DateTime currentDate, 
            double indexPercentage, 
            double newGrossValue,
            double newIncomeTaxValue,
            double newLiquidValue,
            double newInvestmentValue,
            double withdraw)
        {
            var context = await CreateFinanceDataContext();

            //Instancias 
            var titleService = await TitleServiceInstanceAsync();


            var indexesImporter = new IndexImporter(context);

            var holidaysImporter = new HolidaysImporter(context);

            //Importar dados para comparação

            await holidaysImporter.GetHolidays(2022, 2022);

            await indexesImporter.GetIndexes(indexImport: EIndex.CDI, dateStart: new DateTime(2022, 01, 01));


            //------------ Teste 1
            var input = new DefaultTitleInput()
            {
                DateInvestment = dateInvestment,
                Date = currentDate,
                AdditionalFixedInterest = 0,
                Index = EIndex.CDI,
                IndexPercentage = indexPercentage,
                InvestmentValue = investmentValue,
                TypePrivateFixedIncome = ETypePrivateFixedIncome.CDB
            };

            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5

            var value = await titleService.GetCurrentTitleAfterWithdraw(input, withdraw);

            Assert.True(value.titleOutput.GrossValue.ToString("N", SetPrecision) == newGrossValue.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.IncomeTaxValue.ToString("N", SetPrecision) == newIncomeTaxValue.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.LiquidValue.ToString("N", SetPrecision) == newLiquidValue.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.InvestmentValue.ToString("N", SetPrecision) == newInvestmentValue.ToString("N", SetPrecision));

            //----------------- Teste 2

            input = new DefaultTitleInput()
            {
                DateInvestment = new DateTime(2022, 04, 29),
                Date = new DateTime(2022, 06, 07),
                AdditionalFixedInterest = 0,
                Index = EIndex.CDI,
                IndexPercentage = 1.03,
                InvestmentValue = 100,
                TypePrivateFixedIncome = ETypePrivateFixedIncome.CDB
            };

            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5

            value = await titleService.GetCurrentTitleAfterWithdraw(input, 150);

            Assert.True(value.withdraw < 150);
            Assert.True(value.titleOutput.GrossValue.ToString("N", SetPrecision) == 0.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.IncomeTaxValue.ToString("N", SetPrecision) == 0.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.LiquidValue.ToString("N", SetPrecision) == 0.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.InvestmentValue.ToString("N", SetPrecision) == 0.ToString("N", SetPrecision));


            //----------------- Teste 3

            input = new DefaultTitleInput()
            {
                DateInvestment = new DateTime(2022, 04, 29),
                Date = new DateTime(2022, 06, 07),
                AdditionalFixedInterest = 0,
                Index = EIndex.CDI,
                IndexPercentage = 1.03,
                InvestmentValue = 1069.98,
                TypePrivateFixedIncome = ETypePrivateFixedIncome.CDB
            };

            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5

            value = await titleService.GetCurrentTitleAfterWithdraw(input, 100);

            Assert.True(value.titleOutput.GrossValue.ToString("N", SetPrecision) == 983.68.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.IncomeTaxValue.ToString("N", SetPrecision) == 2.85.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.LiquidValue.ToString("N", SetPrecision) == 980.83.ToString("N", SetPrecision));
            Assert.True(value.titleOutput.InvestmentValue.ToString("N", SetPrecision) == 970.98.ToString("N", SetPrecision));


        }
    }
}
