using FinanceApp.Core.Importers;
using FinanceApp.Core.Services;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests.DatesServiceTests
{
    public class IndexValuesTests : TestsBase
    {
        [Fact]
        //Teste insano para garantir que este método está ok
        public async Task MustCalculateSelicValues()
        {
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


            var indexesImporter = new IndexImporter(context);

            var holidaysImporter = new HolidaysImporter(context);
            
            //Importar dados para comparação

            await holidaysImporter.GetHolidays(2022, 2022);

            await indexesImporter.GetIndexes();

            //Importar dados para comparação

            var value = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 01, 04));
            
            var value2 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 02, 01));
            
            var value3 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 03, 01));

            NumberFormatInfo setPrecision = new();
            
            setPrecision.NumberDecimalDigits = 8;

            Assert.True(value.ToString("N", setPrecision) == 1.00034749.ToString("N", setPrecision));
            Assert.True(value2.ToString("N", setPrecision) == 1.00732270.ToString("N", setPrecision));
            Assert.True(value3.ToString("N", setPrecision) == 1.01492840.ToString("N", setPrecision));
            
        }
    }
}