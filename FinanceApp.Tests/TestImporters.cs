using FinanceApp.Core.Importers;
using FinanceApp.Tests.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests
{
    public class TestImporters : TestsBase
    {
        [Fact]
        public async Task TestIndexImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new IndexImporter(dataContext);

            await importer.GetIndexes();

            var values = await dataContext.IndexValues.ToListAsync();

            Assert.True(values.Any());
            Assert.True(values.Select(a => a.Index).Distinct().Count() == importer.Indexes.Count);

            await DeleteDataDb(dataContext);
        }

        [Fact]
        public async Task TestTreasuryBondImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new TreasuryBondImporter(dataContext);

            await importer.GetTreasury();

            var values = await dataContext.TreasuryBondValues.ToListAsync();

            Assert.True(values.Any());

            await DeleteDataDb(dataContext);
        }

        [Fact]
        public async Task TestTreasuryBondTitlesImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new TreasuryBondImporter(dataContext);

            await importer.GetLastValueTreasury();

            var values = await dataContext.TreasuryBondTitles.ToListAsync();

            Assert.True(values.Any());

            await DeleteDataDb(dataContext);
        }
        
        [Fact]
        public async Task TestIndexProspectImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new IndexProspectImporter(dataContext);

            await importer.GetProspectIndexes();

            var values = await dataContext.ProspectIndexValues.ToListAsync();

            Assert.True(values.Any());

            await DeleteDataDb(dataContext);
        }

        [Fact]
        public async Task TestHolidaysImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new HolidaysImporter(dataContext);

            await importer.GetHolidays();

            var values = await dataContext.Holidays.ToListAsync();

            Assert.True(values.Any());

            await DeleteDataDb(dataContext);
        }


        [Fact]
        public async Task TestWorkingDaysImporter()
        {
            var dataContext = await CreateFinanceContext();

            var importer = new WorkingDaysImporter(dataContext);

            await importer.GetWorkingDays();

            var values = await dataContext.WorkingDaysByYear.ToListAsync();

            Assert.True(values.Select(a => a.Year).Min() == 2001);
            Assert.True(values.Select(a => a.Year).Max() > DateTime.Now.Date.AddYears(20).Year);
            Assert.True(values.Any());

            await DeleteDataDb(dataContext);
        }
        
    }
}