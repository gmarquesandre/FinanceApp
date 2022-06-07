using FinanceApp.Core.Importers.Base;
using FinanceApp.EntityFramework;
using FinanceApp.Shared;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace FinanceApp.Core.Importers
{
    public class IndexProspectImporter : ImporterBase
    {
        private HttpClient _client;

        private HttpClientHandler _handler;
        

        public IndexProspectImporter(FinanceContext context) : base(context)
        {

        }

        public async Task GetProspectIndexes()
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            //Swagger da API
            https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/swagger-ui3#/

            await DeleteAllValues();

            await GetIndexes();

            await GetSelic();

            await GetCdi();
            

        }

        private async Task DeleteAllValues()
        {

            var data = _context.ProspectIndexValues.ToList();

            _context.ProspectIndexValues.RemoveRange(data);
            await _context.SaveChangesAsync();
        }

        private async Task GetCdi()
        {
            var valuesSelic = _context.ProspectIndexValues
                .AsNoTracking()
                .Where(a=> a.Index == EIndex.Selic)
                .ToList();

            valuesSelic.ForEach(a =>
            {
                a.Id = 0;
                a.Max -= 0.1;
                a.Min -= 0.1;
                a.Median -= 0.1;
                a.Average -= 0.1;
                a.Index = EIndex.CDI;

            });

            await InsertOrUpdateIndex(valuesSelic);

        }

        private async Task GetSelic()
        {
            var response = await _client.GetAsync("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativasMercadoSelic?%24format=json&%24orderby=Data%20desc&%24top=500");

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic json = JObject.Parse(responseString);

            var values = json.value;

            List<ProspectIndexValue> listValues = new();
            ConvertSelicToList(values, listValues);

            await InsertOrUpdateIndex(listValues);

        }
        private async Task GetIndexes()
        {


            var response = await _client.GetAsync("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/ExpectativaMercadoMensais?%24format=json&%24orderby=Data%20desc&%24top=500");

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic json = JObject.Parse(responseString);


            var values = json.value;
            List<ProspectIndexValue> listValues = new();

            ConvertToList(values, listValues);

            await InsertOrUpdateIndex(listValues);


        }
        private static void ConvertSelicToList(dynamic values, List<ProspectIndexValue> listValues)
        {
            foreach (var value in values)
            {

                string indexName = value.Indicador;
                string minValue = value.Minimo;
                string median = value.Mediana;
                string max = value.Maximo;
                string average = value.Media;
                string numeroRespondentes = value.numeroRespondentes;
                string data = value.Data;
                string reuniao = value.Reuniao;
                string baseCalculo = value.baseCalculo;

                int reuniaoNumero = Convert.ToInt16(reuniao.Substring(1, 1));
                int reuniaoAno = Convert.ToInt16(reuniao.Substring(3, 4));

                //Aproximação grosseira da data
                DateTime dateReferencia = new DateTime(reuniaoAno, 1, 1).AddDays(reuniaoNumero * 45);
                DateTime dateEndReferencia = new DateTime(reuniaoAno, 1, 1).AddDays((reuniaoNumero+1) * 45 - 1);

                try
                {
                    listValues.Add(new ProspectIndexValue
                    {
                        DateStart = dateReferencia,
                        DateResearch = DateTime.ParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateEnd = dateEndReferencia,
                        DateLastUpdate = DateTime.Now,
                        Average = Convert.ToDouble(average, CultureInfo.InvariantCulture),
                        Max = Convert.ToDouble(max, CultureInfo.InvariantCulture),
                        Median = Convert.ToDouble(median, CultureInfo.InvariantCulture),
                        Min = Convert.ToDouble(minValue, CultureInfo.InvariantCulture),
                        ResearchAnswers = Convert.ToInt16(numeroRespondentes),
                        Index = EnumHelper<EIndex>.GetValueFromName(indexName),
                        BaseCalculo = Convert.ToInt16(baseCalculo),
                        IndexRecurrence = EIndexRecurrence.Yearly
                    });
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }

            }
        }
        private static void ConvertToList(dynamic values, List<ProspectIndexValue> listValues)
        {
            foreach (var value in values)
            {

                string indexName = value.Indicador;
                string minValue = value.Minimo;
                string median = value.Mediana;
                string max = value.Maximo;
                string average = value.Media;
                string numeroRespondentes = value.numeroRespondentes;
                string dataReferencia = value.DataReferencia;
                string data = value.Data;
                string baseCalculo = value.baseCalculo;

                try
                {
                    listValues.Add(new ProspectIndexValue
                    {
                        DateStart = DateTime.ParseExact(dataReferencia, "MM/yyyy", CultureInfo.InvariantCulture),
                        DateEnd = DateTime.ParseExact(dataReferencia, "MM/yyyy", CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1),
                        DateResearch = DateTime.ParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateLastUpdate = DateTime.Now,
                        Average = Convert.ToDouble(average, CultureInfo.InvariantCulture),
                        Max = Convert.ToDouble(max, CultureInfo.InvariantCulture),
                        Median = Convert.ToDouble(median, CultureInfo.InvariantCulture),
                        Min = Convert.ToDouble(minValue, CultureInfo.InvariantCulture),
                        ResearchAnswers = Convert.ToInt16(numeroRespondentes),
                        Index = EnumHelper<EIndex>.GetValueFromName(indexName),
                        BaseCalculo = Convert.ToInt16(baseCalculo),
                        IndexRecurrence = EIndexRecurrence.Monthly
                    });
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }

            }
        }             

        private async Task InsertOrUpdateIndex(List<ProspectIndexValue> itens)
        {           
            await InsertValue(itens);
        }

        private async Task InsertValue(List<ProspectIndexValue> list)
        {
            
            list = list.Where(a => a.DateResearch == list.Select(a => a.DateResearch).Max()).ToList();

            await _context.ProspectIndexValues.AddRangeAsync(list);

            await _context.SaveChangesAsync();
        }       
    }
}
