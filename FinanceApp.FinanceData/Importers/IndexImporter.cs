using FinanceApp.Shared.Enum;
using System.Text;
using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.FinanceData.Importers.Base;
using FinanceApp.EntityFramework.Data;

namespace FinanceApp.FinanceData.Importers
{
    public class IndexImporter : ImporterBase
    {
        private HttpClient _client;

        private HttpClientHandler _handler;

        public Dictionary<EIndex, (string code, EIndexRecurrence indexRecurrence)> Indexes = new()
        {
            { EIndex.IPCA, ("433", EIndexRecurrence.Monthly) },
            { EIndex.IGPM, ("189", EIndexRecurrence.Monthly) },
            { EIndex.Selic, ("11", EIndexRecurrence.Daily) },
            { EIndex.CDI, ("12", EIndexRecurrence.Daily) },
            { EIndex.Poupanca, ("196", EIndexRecurrence.Monthly) },
            { EIndex.TR, ("226", EIndexRecurrence.Monthly) },
        };

        public IndexImporter(FinanceDataContext context) : base(context) { }

        public async Task GetIndexes(EIndex? indexImport = null, DateTime? dateStart = null)
        {
            if (indexImport == null)
            {
                foreach (var index in Indexes)
                {
                    await ImportIndex(index, dateStart);
                }
            }
            else
            {
                await ImportIndex(Indexes.FirstOrDefault(a => a.Key == indexImport)!, dateStart);
            }
        }

        private async Task ImportIndex(KeyValuePair<EIndex, (string code, EIndexRecurrence IndexRecurrence)> index, DateTime? dateStart)
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            _client.DefaultRequestHeaders.Host = "api.bcb.gov.br";
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.0.0 Safari/537.36");
            string url = $"http://api.bcb.gov.br/dados/serie/bcdata.sgs.{index.Value.code}/dados?formato=csv";

            DateTime? minDate;
            if (_context.IndexValues.Any(a => a.Index == index.Key))
            {

                minDate = _context.IndexValues.Where(a => a.Index == index.Key).Max(a => a.Date);
                url += $"&dataInicial={minDate.Value:dd/MM/yyyy}&dataFinal=01/01/2999";
            }
            else if (dateStart != null)
            {
                url += $"&dataInicial={dateStart.Value:dd/MM/yyyy}&dataFinal=01/01/2999";
            }
            var response = await _client.GetAsync(url);

            var bytes = await response.Content.ReadAsByteArrayAsync();

            string str = Encoding.Default.GetString(bytes);
            if (str.Contains("<?xml"))
                throw new Exception("Não foi possível buscar dados");
            var itens = ConvertToList(str, index.Key, index.Value.IndexRecurrence);

            await InsertOrUpdateIndex(itens);

        }

        private async Task InsertOrUpdateIndex(List<IndexValue> itens)
        {
            EIndex index = itens.First().Index;

            var allValuesThisIndex = _context.IndexValues.Where(a => a.Index == index).ToList();

            //var listUpdate = allValuesThisIndex.Where(a => itens.Select(b => b.Date).Contains(a.Date)).ToList();

            var listInsert = itens.Where(a => !allValuesThisIndex.Select(b => b.Date).Contains(a.Date)).ToList();

            await InsertValue(listInsert);

            //await UpdateValueAsync(listUpdate);

        }

        //Repensar como fazer um update coerente que não apenas salve o mesmo valor
        //private async Task UpdateValueAsync(List<IndexValue> list)
        //{
        //    _context.IndexValues.UpdateRange(list);

        //    await _context.SaveChangesAsync();
        //}

        private async Task InsertValue(List<IndexValue> list)
        {
            await _context.IndexValues.AddRangeAsync(list);

            await _context.SaveChangesAsync();
        }

        private List<IndexValue> ConvertToList(string str, EIndex index, EIndexRecurrence indexRecurrence)
        {

            var itens = str.Replace("\r", "").Split("\n").Select(a => a.Split(";"))
                .ToList();

            List<string> header = itens.FirstOrDefault()!.Select(a => a.ToLower()).ToList();

            int dateIndex = CheckIfFound(header.IndexOf("\"data\""));
            int dateEndIndex = header.IndexOf("\"datafim\"");
            int valueIndex = CheckIfFound(header.IndexOf("\"valor\""));

            if (dateEndIndex < 0)
                dateEndIndex = dateIndex;

            List<IndexValue> fundValueList = new();

            fundValueList = itens
                //skip header
                .Skip(1)
                //ignore empty itens
                .Where(item => item[0] != "")
            .Select(a => new IndexValue()
            {
                Date = Convert.ToDateTime(a[dateIndex].Replace("\"", ""), _cultureInfoPtBr),
                DateEnd = Convert.ToDateTime(a[dateEndIndex].Replace("\"", ""), _cultureInfoPtBr),
                Index = index,
                IndexRecurrence = indexRecurrence,
                Value = Convert.ToDouble(a[valueIndex].Replace("\"", ""), _cultureInfoPtBr) / 100
            }).ToList();


            return fundValueList;
        }
    }
}
