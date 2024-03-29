//using FinanceApp.Core.Importers;
//using FinanceApp.Core.Services;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Tests.Base;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace FinanceApp.Tests.DatesServiceTests
//{
//    public class DataServiceTests : TestsBase
//    {
//        [Fact]
//        //Teste insano para garantir que este m�todo est� ok
//        public async Task MustCalculateWorkingDaysBetweenDatesCorrectly()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateUserContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var indexesImporter = new IndexImporter(context);

//            var holidaysImporter = new HolidaysImporter(context);

//            //Importar dados para compara��o

//            await holidaysImporter.GetHolidays(2002,DateTime.Now.Year);

//            await indexesImporter.GetIndexes();

//            // pega dados para referencia

//            var selicIndexes = context.IndexValues.Where(a => a.Index == EIndex.Selic && a.Date.Year >= 2001);

//            // pega resultado do m�todo
//            for (DateTime date = new(2002,1,1); date < DateTime.Now.AddYears(-1).AddDays(-1); date = date.AddDays(1))
//            {

//                DateTime dateStart = date;
//                DateTime dateEnd = date.AddYears(1);

//                var result = await datesService.GetWorkingDaysBetweenDates(dateStart, dateEnd);

//                int resultCompare = selicIndexes.Where(a => a.Date >= dateStart && a.Date <= dateEnd).Count();
               
//                Assert.True(result == resultCompare);

//            }

//        }


//        [Fact]
//        //Teste insano para garantir que este m�todo est� ok
//        public async Task MustReturnAddWorkingDaysCorrectly()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateUserContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var holidaysImporter = new HolidaysImporter(context);

//            //Importar dados para compara��o

//            await holidaysImporter.GetHolidays(2022, 2022);



//            //---------------------------------------
//            DateTime newDate = await datesService.AddWorkingDays(new DateTime(2022, 06, 20), 1);

//            Assert.True(newDate == new DateTime(2022, 06, 21));
//            //---------------------------------------
//            newDate = await datesService.AddWorkingDays(new DateTime(2022, 06, 20), -1);

//            Assert.True(newDate == new DateTime(2022, 06, 17));
//            //---------------------------------------
//            newDate = await datesService.AddWorkingDays(new DateTime(2022, 06, 20), -2);

//            Assert.True(newDate == new DateTime(2022, 06, 15));
//            //---------------------------------------
//            newDate = await datesService.AddWorkingDays(new DateTime(2022, 06, 20), 0);

//            Assert.True(newDate == new DateTime(2022, 06, 20));
//            //---------------------------------------
//            newDate = await datesService.AddWorkingDays(new DateTime(2022, 06, 20), 5);

//            Assert.True(newDate == new DateTime(2022, 06, 27));
//            //---------------------------------------

//        }


//    }
//}