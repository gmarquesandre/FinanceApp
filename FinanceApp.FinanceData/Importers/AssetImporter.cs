﻿using Hangfire;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;
using FinanceApp.FinanceData.Services;
using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.FinanceData.Importers.Base;
using FinanceApp.EntityFramework.Data;
using FinanceApp.Shared.Enum;

namespace FinanceApp.FinanceData.Importers
{
    public class AssetImporter : ImporterBase
    {
        private HttpClient _client = new();
        private HttpClientHandler _handler = new();
        private CultureInfo _cultureInfo = new("pt-br");
        public IDatesService _dateService;
        public AssetImporter(FinanceDataContext context, IDatesService dates) : base(context)
        {
            _context = context;
            _dateService = dates;

        }

        [AutomaticRetry(Attempts = 0)]
        public async Task GetAssets()
        {

            var dates = _context.Assets.Select(a => a.Date);

            DateTime dateStart = new(DateTime.Now.Year, 01, 01);
            DateTime dateEnd = DateTime.Now;

            for (int year = 2000; year < DateTime.Now.Year; year += 1)
            {
                BackgroundJob.Enqueue<AssetImporter>(a => a.GetAssetsWithYear(year));

            }

            for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
            {
                if (await _dateService.IsHolidayOrWeekend(date) || _context.Assets.Where(a => a.Date == date).First() != null)
                    continue;

                BackgroundJob.Enqueue<AssetImporter>(a => a.GetAssetsWithDate(date));


            }

        }


        [AutomaticRetry(Attempts = 0)]
        [Queue("asset-year")]
        public async Task GetAssetsWithYear(int year)
        {
            _handler = SetDefaultHttpHandler();
            //https://www.b3.com.br/data/files/C8/F3/08/B4/297BE410F816C9E492D828A8/SeriesHistoricas_Layout.pdf
            _client = new HttpClient(_handler);

            //https://www.b3.com.br/pt_br/market-data-e-indices/servicos-de-dados/market-data/historico/mercado-a-vista/series-historicas/

            //https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_A{year}.ZIP para anuaç
            var response = await _client.GetAsync($"https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_A{year}.ZIP");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Arquivo não encontrado para o ano {year}");

            var fileStream = await response.Content.ReadAsStreamAsync();

            string fileString = Unzip(fileStream);

            List<string> rows = fileString.Split('\n').Skip(1).Reverse().Skip(2).ToList();

            //010 filtro para apenas ações comuns, sem opções
            List<Asset> assetList = rows.Where(a => a.Substring(24, 3) == "010").Select(a => MapToAsset(a)).ToList();


            await InserOrUpdateAsset(assetList);
        }

        [AutomaticRetry(Attempts = 0)]
        [Queue("asset-date")]
        public async Task GetAssetsWithDate(DateTime date)
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            string dayMonthYear = date.ToString("ddMMyyyy");

            //https://www.b3.com.br/pt_br/market-data-e-indices/servicos-de-dados/market-data/historico/mercado-a-vista/series-historicas/

            //https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_A{year}.ZIP para anuaç
            var response = await _client.GetAsync($"https://bvmf.bmfbovespa.com.br/InstDados/SerHist/COTAHIST_D{dayMonthYear}.ZIP");

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Arquivo não encontrado para a Data {date:dd/MM/yyyy}");

            var fileStream = await response.Content.ReadAsStreamAsync();

            string fileString = Unzip(fileStream);

            List<string> rows = fileString.Split('\n').Skip(1).Reverse().Skip(2).ToList();

            //010 filtro para apenas ações comuns, sem opções
            List<Asset> assetList = rows.Where(a => a.Substring(24, 3) == "010").Select(a => MapToAsset(a)).ToList();


            await InserOrUpdateAsset(assetList);
        }

        private async Task InserOrUpdateAsset(List<Asset> assetList)
        {
            var dates = assetList.Select(a => a.Date).ToList();

            var datesAlreadyOnDb = _context.Assets.Select(a => a.Date).Distinct().ToList();

            var listInsert = assetList.Where(a => !datesAlreadyOnDb.Contains(a.Date)).ToList();


            await InsertAsset(listInsert);

            //await UpdateAsset(listUpdate);

        }

        private async Task UpdateAsset(List<Asset> listUpdate)
        {
            _context.UpdateRange(listUpdate);
            await _context.SaveChangesAsync();
        }

        private async Task InsertAsset(List<Asset> listInsert)
        {
            await _context.Assets.AddRangeAsync(listInsert);
            await _context.SaveChangesAsync();
        }

        private static Asset MapToAsset(string a)
        {
            return new Asset()
            {
                AssetCode = a.Substring(12, 12).Trim(),
                AssetCodeISIN = a.Substring(230, 12),
                CompanyName = a.Substring(27, 12).Trim(),
                Date = DateTime.ParseExact(a.Substring(2, 8), "yyyyMMdd", CultureInfo.InvariantCulture),
                ClosingPrice = Convert.ToDouble(a.Substring(108, 13)) / 100,
                AveragePrice = Convert.ToDouble(a.Substring(95, 13)) / 100,
                OpeningPrice = Convert.ToDouble(a.Substring(56, 13)) / 100,
                MaxPrice = Convert.ToDouble(a.Substring(69, 13)) / 100,
                MinPrice = Convert.ToDouble(a.Substring(82, 13)) / 100,
                StockTradeCount = Convert.ToDouble(a.Substring(152, 20)),
                TradeCount = Convert.ToDouble(a.Substring(147, 7))

            };
        }

        private static string Unzip(Stream streammedFile)
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
