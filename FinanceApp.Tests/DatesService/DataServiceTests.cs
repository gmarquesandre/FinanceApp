using FinanceApp.Core.Importers;
using FinanceApp.Core.Services.DataServices;
using FinanceApp.Shared.Enum;
using FinanceApp.Tests.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests
{
    public class DataServiceTests : TestsBase
    {
        [Fact]
        //Teste insano para garantir que este método está ok
        public async Task MustCalculateWorkingDaysBetweenDatesCorrectly()
        {
            var mapper = GetConfigurationIMapper();
            var context = await CreateFinanceContext();

            //Instancias 
            MemoryCacheOptions cacheOptions = new();            
            var memoryCache = new MemoryCache(cacheOptions);
            var datesService = new DatesService(context, mapper, memoryCache);
            var indexesImporter = new IndexImporter(context);
            var holidaysImporter = new HolidaysImporter(context);

            //Importar dados para comparação

            await holidaysImporter.GetHolidays(DateTime.Now.Year);

            await indexesImporter.GetIndexes();

            // pega dados para referencia

            var selicIndexes = context.IndexValues.Where(a => a.Index == EIndex.Selic && a.Date.Year >= 2001);

            // pega resultado do método
            DateTime dateStart = new(2010, 01, 01);
            DateTime dateEnd = new(2011, 05, 01);
            var result = await datesService.GetWorkingDaysBetweenDates(dateStart, dateEnd);

            int resultCompare = selicIndexes.Where(a => a.Date >= dateStart && a.Date <= dateEnd).Count();

            Assert.True(result == resultCompare);

            var dataContext = await CreateFinanceContext();

            var importer = new IndexImporter(dataContext);

            await importer.GetIndexes();

            var values = await dataContext.IndexValues.ToListAsync();

            Assert.True(values.Any());
            Assert.True(values.Select(a => a.Index).Distinct().Count() == importer.Indexes.Count);

            await DeleteDataDb(dataContext);
        }        
    }
}