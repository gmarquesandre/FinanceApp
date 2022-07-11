//using FinanceApp.Core.Importers;
//using FinanceApp.Core.Services;
//using FinanceApp.Shared.Enum;
//using FinanceApp.Shared.Models.CommonTables;
//using FinanceApp.Tests.Base;
//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Threading.Tasks;
//using Xunit;

//namespace FinanceApp.Tests.DatesServiceTests
//{
//    public class IndexValuesTests : TestsBase
//    {
//        [Fact]
//        public async Task MustCalculateSelicValues()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateFinanceContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var indexService = new IndexService(context,
//                                                mapper,
//                                                memoryCache,
//                                                datesService);


//            var indexesImporter = new IndexImporter(context);

//            var holidaysImporter = new HolidaysImporter(context);
            
//            //Importar dados para comparação

//            await holidaysImporter.GetHolidays(2022, 2022);

//            await indexesImporter.GetIndexes(indexImport: EIndex.Selic, dateStart: new DateTime(2022, 01, 01));

//            //Importar dados para comparação

//            var value = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 01, 04));
            
//            var value2 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 02, 01));
            
//            var value3 = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 1, 1), new DateTime(2022, 03, 01));

//            NumberFormatInfo setPrecision = new();
            
//            setPrecision.NumberDecimalDigits= 8;

//            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5
//            Assert.True(value.ToString("N", setPrecision) == 1.00034749.ToString("N", setPrecision));
//            Assert.True(value2.ToString("N", setPrecision) == 1.00732270.ToString("N", setPrecision));
//            Assert.True(value3.ToString("N", setPrecision) == 1.01492840.ToString("N", setPrecision));
            
//        }


//        [Fact]
//        public async Task MustCalculateSelicValuesWithFutureValues()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateFinanceContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var indexService = new IndexService(context,
//                                                mapper,
//                                                memoryCache,
//                                                datesService);

//            var holidaysImporter = new HolidaysImporter(context);

//            //Importar dados para comparação

//            await holidaysImporter.GetHolidays(2022, 2022);
            
//            //Importar dados para comparação

//            context.ProspectIndexValues.Add(new ProspectIndexValue()
//            {
//                Average = 0,
//                Median = 12.649937272,
//                BaseCalculo = 0,
//                DateEnd = new DateTime(2022, 6, 1),
//                DateStart = new DateTime(2022, 5, 5),
//                Index = EIndex.Selic,
//                IndexRecurrence = EIndexRecurrence.Yearly,
//            });
//            context.WorkingDaysByYear.Add(new WorkingDaysByYear() {
//                WorkingDays = 252,
//                Year = 2022
//            });
//            context.IndexValues.AddRange(new List<IndexValue>() {
//                new IndexValue()
//                {
//                    Index = EIndex.Selic,
//                    IndexRecurrence = EIndexRecurrence.Daily,
//                    Date = new DateTime(2022,3,17),
//                    DateEnd = new DateTime(2022,3,17),
//                    Value = 0.00043739                    
//                },
//            });

//            context.SaveChanges();

//            var value = await indexService.GetIndexValueBetweenDates(EIndex.Selic, new DateTime(2022, 3, 17), new DateTime(2022, 6, 1));

//            NumberFormatInfo setPrecision = new();

//            setPrecision.NumberDecimalDigits = 6;

//            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5
//            Assert.True(value.ToString("N", setPrecision) == 1.02368780.ToString("N", setPrecision));

//        }

//        [Fact]
//        public async Task MustCalculateCdiValue()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateFinanceContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var indexService = new IndexService(context,
//                                                mapper,
//                                                memoryCache,
//                                                datesService);


//            var indexesImporter = new IndexImporter(context);

//            var holidaysImporter = new HolidaysImporter(context);

//            //Importar dados para comparação

//            await holidaysImporter.GetHolidays(2022, 2022);

//            await indexesImporter.GetIndexes(indexImport: EIndex.CDI, dateStart: new DateTime(2022, 01, 01));

//            //Importar dados para comparação

//            var value = await indexService.GetIndexValueBetweenDates(EIndex.CDI, new DateTime(2022, 05, 31), new DateTime(2022, 06, 17), 1.03);

//            NumberFormatInfo setPrecision = new();

//            setPrecision.NumberDecimalDigits = 8;

//            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5
//            Assert.True(value.ToString("N", setPrecision) == 1.00585936.ToString("N", setPrecision));
           
//        }


//        [Fact]
//        public async Task MustCalculateIPCAValues()
//        {
//            var mapper = GetConfigurationIMapper();
//            var context = await CreateFinanceContext();

//            //Instancias 
//            MemoryCacheOptions cacheOptions = new();

//            var memoryCache = new MemoryCache(cacheOptions);

//            var datesService = new DatesService(context,
//                                                mapper,
//                                                memoryCache);


//            var indexService = new IndexService(context,
//                                                mapper,
//                                                memoryCache,
//                                                datesService);


//            var indexesImporter = new IndexImporter(context);

//            var holidaysImporter = new HolidaysImporter(context);

//            //Importar dados para comparação

//            await holidaysImporter.GetHolidays(2022, 2022);

//            await indexesImporter.GetIndexes(indexImport: EIndex.IPCA, dateStart: new DateTime(2022, 01, 01));

//            //Importar dados para comparação

//            var value = await indexService.GetIndexValueBetweenDates(EIndex.IPCA, new DateTime(2022, 1, 1), new DateTime(2022, 03, 31));
            
//            NumberFormatInfo setPrecision = new();

//            setPrecision.NumberDecimalDigits = 6;

//            //Valores extaidos da calculadora do cidadão para CDI de 100% https://www3.bcb.gov.br/CALCIDADAO/publico/exibirFormCorrecaoValores.do?method=exibirFormCorrecaoValores&aba=5

//            Assert.True(value.ToString("N", setPrecision) == 1.03200650.ToString("N", setPrecision));
            
//        }

//    }
//}