using FinanceApp.Shared.Enum;
using FinancialAPI.Data;
using System.Text;

namespace FinanceApp.Core.Importers
{
    public class IndexImporter : ImporterBase
    {
        private HttpClient _client;

        private HttpClientHandler _handler;

        private Dictionary<EIndex, string> Indexes = new()
        {
            { EIndex.IPCA, "433" },
            { EIndex.IGPM, "189" },
            { EIndex.Selic, "11" },
            { EIndex.CDI, "12" },
            { EIndex.Poupanca, "196" },
            { EIndex.TR, "226" },
        };
        public async Task ImportIndexes()
        {
            foreach(var index in Indexes)
            {
                await ImportIndex(index);
            }
        }
        
        private async Task ImportIndex(KeyValuePair<EIndex, string> index)
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            _client.DefaultRequestHeaders.Host = "api.bcb.gov.br";
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.0.0 Safari/537.36");

            var response = await _client.GetAsync($"http://api.bcb.gov.br/dados/serie/bcdata.sgs.{index.Value}/dados?formato=csv");

            var bytes = await response.Content.ReadAsByteArrayAsync();

            string str = Encoding.Default.GetString(bytes);

            var itens = ConvertToList(str, index.Key);

            await InsertOrUpdateIndex(itens);

        }

        private async Task InsertOrUpdateIndex(List<IndexValue> itens)
        {

            itens = itens.Where(a => a.Date >= new DateTime(2010, 1, 1)).ToList();
            EIndex index = itens.First().Index;
            
                var allValuesThisIndex = _context.IndexValues.Where(a => a.Index == index).ToList();

                var listUpdate = allValuesThisIndex.Where(a => itens.Select(b => b.Date).Contains(a.Date)).ToList();

                var listInsert = itens.Where(a => !allValuesThisIndex.Select(b => b.Date).Contains(a.Date)).ToList();

                await InsertValue(listInsert);

                await UpdateValueAsync(listUpdate);
            
        }

        private async Task UpdateValueAsync(List<IndexValue> list)
        {
            _context.IndexValues.UpdateRange(list);

            await _context.SaveChangesAsync();
        }

        private async Task InsertValue(List<IndexValue> list)
        {
            await _context.IndexValues.AddRangeAsync(list);

            await _context.SaveChangesAsync();
        }

        private List<IndexValue> ConvertToList(string str, EIndex index)
        {

            var itens = str.Replace("\r", "").Split("\n").Select(a => a.Split(";"))
                .ToList();

            List<string> header = itens.FirstOrDefault()!.Select(a => a.ToLower()).ToList();

            int dateIndex = header.IndexOf("\"data\"");
            int dateEndIndex = header.IndexOf("\"datafim\"");
            int valueIndex = header.IndexOf("\"valor\"");


            if (dateEndIndex == -1 && index != EIndex.TR)
                dateEndIndex = dateIndex;

            List<int> indexes = new()
            {
                dateIndex,
                dateEndIndex,
                valueIndex

            };

            if (indexes.Any(a => a == -1))
                throw new Exception("Column not found");

            List<IndexValue> fundValueList = new();

            fundValueList = itens
                //skip header
                .Skip(1)
                //ignore empty itens
                .Where(item => item[0] != "")
            .Select(a => new IndexValue()
            {
                Date = Convert.ToDateTime(a[dateIndex].Replace("\"",""), _cultureInfoPtBr),
                DateEnd = Convert.ToDateTime(a[dateEndIndex].Replace("\"",""), _cultureInfoPtBr),
                Index = index,
                Value = Convert.ToDouble(a[valueIndex].Replace("\"", ""), _cultureInfoPtBr) /100                
            }).ToList();
           

            return fundValueList;
        }
    }
}
