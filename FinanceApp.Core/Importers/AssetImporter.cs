using AngleSharp.Html.Parser;
using FinancialApi.WebAPI.Data;
using FinancialAPI.Data;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace FinanceApp.Core.Importers
{
    public class AssetImporter
    {
        private FinanceContext _context = new FinanceContext();
        private HttpClient _client = new();
        private HtmlParser _parser = new();
        private CultureInfo _cultureInfo = new("pt-br");
        public AssetImporter()
        {

        }

        public async Task GetAssets(DateTime? date = null)
        {
            if (date == null)
                date = DateTime.Now.Date;

            string dayMonthYear = date.Value.ToString("ddMMyyyy");

            var response = await _client.GetAsync($"https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_D{dayMonthYear}.ZIP");

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception($"Arquivo não encontrado para a Data {date.Value:dd/MM/yyyy}");

            var fileStream = await response.Content.ReadAsStreamAsync();

            string fileString = Unzip(fileStream);

            List<string> rows = fileString.Split('\n').Skip(1).Reverse().Skip(2).ToList();

            //010 filtro para apenas ações comuns, sem opções
            List<Asset> assetList = rows.Where(a => a.Substring(24, 3) == "010").Select(a => MapToAsset(a, date.Value)).ToList();           


            await InserOrUpdateAsset(assetList);


        }

        private async Task InserOrUpdateAsset(List<Asset> assetList)
        {
            var listUpdate = _context.Assets.Where(a => assetList.Select(a => a.AssetCodeISIN).Contains(a.AssetCodeISIN)).ToList();

            var listInsert = assetList.Where(a => !listUpdate.Select(a => a.AssetCodeISIN).Contains(a.AssetCodeISIN)).ToList();

            await InserAsset(listInsert);

            await UpdateAsset(listUpdate);

        }

        private async Task UpdateAsset(List<Asset> listUpdate)
        {
            _context.UpdateRange(listUpdate);
            await _context.SaveChangesAsync();
        }

        private async Task InserAsset(List<Asset> listInsert)
        {
            await _context.Assets.AddRangeAsync(listInsert);
            await _context.SaveChangesAsync();
        }

        private Asset MapToAsset(string a, DateTime date)
        {
           
            
                return new Asset()
                {
                    AssetCode = a.Substring(12, 12).Trim(),
                    AssetCodeISIN = a.Substring(230, 12),
                    CompanyName = a.Substring(27, 12).Trim(),
                    DateLastUpdate = date,
                    UnitPrice = Convert.ToDouble(a.Substring(108, 13)) / 100
                };
            
         
            
        }

        private string Unzip(Stream streammedFile)
        {
            StringBuilder resp = new StringBuilder();

            using (var zip = new ZipArchive(streammedFile, ZipArchiveMode.Read, false, Encoding.GetEncoding("ISO-8859-1")))
            {
                foreach (var archive in zip.Entries)
                {
                    var fileExported = archive.Open();
                    StreamReader newReader = new StreamReader(fileExported);

                    resp.Append(newReader.ReadToEnd());
                }
            }

            return resp.ToString();
        }

    }
}
