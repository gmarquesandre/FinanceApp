using FinanceApp.Tests.Base;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.Forecast
{
    public class ForecastTests : TestsBase
    {






        [Fact]
        public async Task MustReturnCorrectCurrentBalanceValueAsync()
        {


            //GetCurrentValueOfTitle(DateTime dateInvestment, decimal investmentValue, DateTime date, bool updateWithCdiIndex, double indexPercentage)
            var mapper = GetConfigurationIMapper();
            var context = await CreateFinanceContext();

            //Instancias 
            MemoryCacheOptions cacheOptions = new();

            var memoryCache = new MemoryCache(cacheOptions);

            var forecastService = new ForecastService(context,
                                                mapper,
                                                memoryCache);


            var indexService = new IndexService(context,
                                                mapper,
                                                memoryCache,
                                                datesService);


            var indexesImporter = new IndexImporter(context);

            var holidaysImporter = new HolidaysImporter(context);

            //Importar dados para comparação

            await holidaysImporter.GetHolidays(2022, 2022);

            await indexesImporter.GetIndexes(indexImport: EIndex.Selic, dateStart: new DateTime(2022, 01, 01));

            //Importar dados para comparação

            var value = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 01, 04));

            var value2 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 02, 01));

            var value3 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 03, 01));

            NumberFormatInfo setPrecision = new();

            setPrecision.NumberDecimalDigits = 8;

            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5
            Assert.True(value.ToString("N", setPrecision) == 1.00034749.ToString("N", setPrecision));
            Assert.True(value2.ToString("N", setPrecision) == 1.00732270.ToString("N", setPrecision));
            Assert.True(value3.ToString("N", setPrecision) == 1.01492840.ToString("N", setPrecision));



        }
    }
}
