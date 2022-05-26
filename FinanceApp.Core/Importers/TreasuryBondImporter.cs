using FinanceApp.Shared;
using FinanceApp.Shared.Enum;
using FinancialApi.WebAPI.Data;
using FinancialAPI.Data;
using System.Text;

namespace FinanceApp.Core.Importers
{
    public class TreasuryBondImporter : ImporterBase
    {
        public HttpClient _client = new();

        public TreasuryBondImporter(FinanceContext context): base(context)
        {

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

            await InsertOrUpdateIndex(itens);

        }

        private async Task InsertOrUpdateIndex(List<TreasuryBondValue> itens)
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
                FixedInterestValueBuy = Convert.ToDouble(a[interestRateBuy]),
                FixedInterestValueSell = Convert.ToDouble(a[interestRateSell]),
                UnitPriceBuy = Convert.ToDouble(a[unitPriceBuy]),
                UnitPriceSell = Convert.ToDouble(a[unitPriceSell]),
                
                //DateEnd = Convert.ToDateTime(a[dateEndIndex].Replace("\"", ""), _cultureInfoPtBr),
                //    Index = index,
                //    Value = Convert.ToDouble(a[valueIndex].Replace("\"", ""), _cultureInfoPtBr) / 100
            }).ToList();

            //Remove titulos vencimentos a mais de 1 mês
            if(removeExpired)
                treasuryBondValueList = treasuryBondValueList.Where(a => a.ExpirationDate <= DateTime.Now.AddDays(-1)).ToList();
            
            return treasuryBondValueList;
        }

    }
}
