using FinanceApp.Core.Importers;
using FinanceApp.Core.Services;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using static FinanceApp.Core.Services.TitleService;

namespace FinanceApp.Tests.TitleTests
{
    public class ForecastTests : TestsBase
    {
        [Fact]
        public async Task MustReturnCorrectCurrentBalanceValueAsync()
        {


            //GetCurrentValueOfTitle(DateTime dateInvestment, double investmentValue, DateTime date, bool updateWithCdiIndex, double indexPercentage)
            var mapper = GetConfigurationIMapper();
            var context = await CreateFinanceContext();

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


            var indexesImporter = new IndexImporter(context);

            var holidaysImporter = new HolidaysImporter(context);

            //Importar dados para comparação

            await holidaysImporter.GetHolidays(2022, 2022);

            await indexesImporter.GetIndexes(indexImport: EIndex.CDI, dateStart: new DateTime(2022, 01, 01));


            //Precisão
            NumberFormatInfo setPrecision = new();

            setPrecision.NumberDecimalDigits = 2;


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

            Assert.True(value.GrossValue.ToString("N", setPrecision) == 398.17.ToString("N", setPrecision));


            //-----------------------

            input.InvestmentValue = 345.82;
            input.DateInvestment = new DateTime(2022, 05, 31);

            value = await titleService.GetCurrentValueOfTitle(input);

            Assert.True(value.GrossValue.ToString("N", setPrecision) == 348.02.ToString("N", setPrecision));


            //-----------------------

            input.InvestmentValue = 633.11;
            input.DateInvestment = new DateTime(2022, 04, 11);
            value = await titleService.GetCurrentValueOfTitle(input);

            Assert.True(value.GrossValue.ToString("N", setPrecision) == 647.39.ToString("N", setPrecision));

        }
    }
}
