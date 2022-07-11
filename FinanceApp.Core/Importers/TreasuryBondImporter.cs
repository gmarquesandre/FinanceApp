using FinanceApp.Core.Importers.Base;
using FinanceApp.Api;
using FinanceApp.Shared;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FinanceApp.Core.Importers
{
    public class TreasuryBondImporter : ImporterBase
    {
        public HttpClient _client = new();

        public TreasuryBondImporter(FinanceContext context): base(context)
        {

        }
        public async Task GetLastValueTreasury()
        {

            var response = await _client.GetAsync("https://www.tesourodireto.com.br/json/br/com/b3/tesourodireto/service/api/treasurybondsinfo.json");

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic json = JObject.Parse(responseString);

            var values = json.response.TrsrBdTradgList;

            DateTime lastUpdate = Convert.ToDateTime(json.response.TrsrBondMkt.opngDtTm);

            List<TreasuryBondTitle> listValues = new();

            ConvertToList(values, listValues, lastUpdate);

            //await InsertOrUpdate(listValues);

            List<TreasuryBondValue> listValuesHistory = new();

            foreach (var value in listValues)
            {
                listValuesHistory.Add(new TreasuryBondValue(){
                   Date = value.LastUpdateDateTime.Date,
                   FixedInterestValueBuy = value.FixedInterestValueBuy,
                   FixedInterestValueSell = value.FixedInterestValueSell,
                   ExpirationDate = value.ExpirationDate,
                   Type = value.Type,
                   UnitPriceBuy = value.UnitPriceBuy,
                   UnitPriceSell = value.UnitPriceSell
                });
            }

            await InsertOrUpdate(listValuesHistory);
        }

        //private async Task InsertOrUpdate(List<TreasuryBondTitle> listValues)
        //{

        //    var allValues = await _context.TreasuryBondTitles.AsNoTracking().ToListAsync();

        //    var listInsert = listValues
        //            .Where(a => !allValues.Select(b => b.KeyTitle()).Contains(a.KeyTitle())).ToList();

        //    var listUpdate = listValues.Where(a => allValues.Select(b => b.KeyTitle()).Contains(a.KeyTitle())).ToList();



        //    await InsertValues(listInsert);
        //    await UpdateValues(listUpdate);
        //}

        //private async Task UpdateValues(List<TreasuryBondTitle> listUpdate)
        //{
        //    foreach (var treasuryBondTitle in listUpdate)
        //    {
        //        var title = await _context.TreasuryBondTitles.AsNoTracking()
        //            .FirstOrDefaultAsync(a => a.Type == treasuryBondTitle.Type && a.ExpirationDate == treasuryBondTitle.ExpirationDate);

        //        treasuryBondTitle.Id = title!.Id;

        //        _context.TreasuryBondTitles.Update(treasuryBondTitle);
        //    }
        //    await _context.SaveChangesAsync();    
        //}

        //private async Task InsertValues(List<TreasuryBondTitle> listInsert)
        //{
        //    await _context.TreasuryBondTitles.AddRangeAsync(listInsert);
        //    await _context.SaveChangesAsync();
        //}

        private void ConvertToList(dynamic values, List<TreasuryBondTitle> listValues, DateTime dateLastUpdate)
        {
            foreach(var value in values)
            {
                var obj = value.TrsrBd;
                DateTime expirationDate = Convert.ToDateTime(obj.mtrtyDt);
                string fixedInterestValueBuy = obj.anulInvstmtRate;
                string fixedInterestValueSell = obj.anulRedRate;
                string description = obj.featrs;
                string titleNameWithYear = obj.nm.ToString();
                string titleName = titleNameWithYear[..titleNameWithYear.LastIndexOf(' ')];
                string unitPriceBuy = obj.untrInvstmtVal;
                string unitPriceSell = obj.untrRedVal;

                listValues.Add(new TreasuryBondTitle()
                {
                    ExpirationDate = expirationDate,
                    FixedInterestValueBuy= Convert.ToDouble(fixedInterestValueBuy, _cultureInvariant),
                    FixedInterestValueSell= Convert.ToDouble(fixedInterestValueSell, _cultureInvariant),
                    LastUpdateDateTime = dateLastUpdate,
                    Description = description,
                    Type = EnumHelper<ETreasuryBond>.GetValueFromName(titleName),
                    UnitPriceBuy = Convert.ToDouble(unitPriceBuy, _cultureInvariant),
                    UnitPriceSell = Convert.ToDouble(unitPriceSell, _cultureInvariant)                    
                });
            }
        }

        public async Task GetTreasury()
        {
            //Documentação dos dados
            //https://www.tesourotransparente.gov.br/ckan/dataset/taxas-dos-titulos-ofertados-pelo-tesouro-direto

            var url = "https://www.tesourotransparente.gov.br/ckan/dataset/df56aa42-484a-4a59-8184-7676580c81e3/resource/796d2059-14e9-44e3-80c9-2d9e30b405c1/download/PrecoTaxaTesouroDireto.csv";
                       
            var response = await _client.GetAsync(url);

            var bytes = await response.Content.ReadAsByteArrayAsync();

            string str = Encoding.Default.GetString(bytes);

            var itens = ConvertToList(str);

            await InsertOrUpdate(itens);

        }

        private async Task InsertOrUpdate(List<TreasuryBondValue> itens)
        {
            var treasuryTypeList = Enum.GetValues(typeof(ETreasuryBond)).Cast<ETreasuryBond>().ToList();
           
            foreach(var type in treasuryTypeList)
            {
                var itensType = itens.Where(a => a.Type == type).ToList();
                
                var allValues = _context.TreasuryBondValues.Where(a=> a.Type == type).ToList();

                //var listUpdate = itensType
                //    .Where(a => allValues.Select(b => b.Key()).Contains(a.Key())).ToList();

                var listInsert = itensType
                    .Where(a => !allValues.Select(b => b.Key()).Contains(a.Key())).ToList();

                await InsertValue(listInsert);

                //await UpdateValueAsync(listUpdate);
            }
            
        }


        //private async Task UpdateValueAsync(List<TreasuryBondValue> list)
        //{                        
        //    _context.TreasuryBondValues.UpdateRange(list);

        //    await _context.SaveChangesAsync();
        //}

        private async Task InsertValue(List<TreasuryBondValue> list)
        {
            await _context.TreasuryBondValues.AddRangeAsync(list);

            await _context.SaveChangesAsync();
        }

        
        private List<TreasuryBondValue> ConvertToList(string str,bool removeExpired = true)
        {

            var itens = str.Replace("\r", "").Split("\n").Select(a => a.Split(";"))
                .ToList();

            List<string> header = itens.FirstOrDefault()!.Select(a => a.ToLower()).ToList();
            //Tipo Titulo; Data Vencimento; Data Base; Taxa Compra Manha; Taxa Venda Manha; PU Compra Manha; PU Venda Manha; PU Base Manha            

            int dateIndex = CheckIfFound(header.IndexOf("data base"));
            int typeIndex = CheckIfFound(header.IndexOf("tipo titulo"));
            int expirationDate = CheckIfFound(header.IndexOf("data vencimento"));                        
            int interestRateBuy = CheckIfFound(header.IndexOf("taxa compra manha"));                        
            int interestRateSell  = CheckIfFound(header.IndexOf("taxa venda manha"));                        
            int unitPriceSell = CheckIfFound(header.IndexOf("pu venda manha"));                        
            int unitPriceBuy = CheckIfFound(header.IndexOf("pu compra manha"));                        
            int unitPriceBase = CheckIfFound(header.IndexOf("pu base manha"));
      

            List<TreasuryBondValue> treasuryBondValueList = new();

            treasuryBondValueList = itens
                //skip header
                .Skip(1)
                //ignore empty itens
                .Where(item => item[0] != "")
            .Select(a => new TreasuryBondValue()
            {
                Date = Convert.ToDateTime(a[dateIndex].Replace("\"", ""), _cultureInfoPtBr),
                Type = EnumHelper<ETreasuryBond>.GetValueFromName(a[typeIndex]),
                ExpirationDate = Convert.ToDateTime(a[expirationDate].Replace("\"", ""), _cultureInfoPtBr),
                FixedInterestValueBuy = Convert.ToDouble(a[interestRateBuy].Replace(",", "."), _cultureInvariant),
                FixedInterestValueSell = Convert.ToDouble(a[interestRateSell].Replace(",", "."), _cultureInvariant),
                UnitPriceBuy = Convert.ToDouble(a[unitPriceBuy].Replace(",","."), _cultureInvariant),
                UnitPriceSell = Convert.ToDouble(a[unitPriceSell].Replace(",", "."), _cultureInvariant),
                
                //DateEnd = Convert.ToDateTime(a[dateEndIndex].Replace("\"", ""), _cultureInfoPtBr),
                //    Index = index,
                //    Value = Convert.ToDouble(a[valueIndex].Replace("\"", ""), _cultureInfoPtBr) / 100
            }).ToList();

            DateTime maxDate = treasuryBondValueList.Max(a => a.Date);

            //Remove titulos vencimentos a mais de 1 mês
            if(removeExpired)
                treasuryBondValueList = treasuryBondValueList.Where(a => a.ExpirationDate >= DateTime.Now.AddDays(-1)).ToList();
            
            return treasuryBondValueList;
        }

    }
}
