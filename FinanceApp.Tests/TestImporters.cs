using FinanceApp.Core.Importers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FinanceApp.Tests
{
    public class TestImporters : DbTests
    {
        [Fact]
        public async Task TestIndexImport()
        {
            var dataContext = await CreateDataDbContext();

            var indexImporter = new IndexImporter(dataContext);

            await indexImporter.ImportIndexes();

            var values = await dataContext.IndexValues.ToListAsync();

            Assert.True(values.Any());
            Assert.True(values.Select(a=> a.Index).Distinct().Count() == indexImporter.Indexes.Count());
        }
    }
}