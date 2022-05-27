using FinancialApi.WebAPI.Data;
using FinancialAPI.Data;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace FinanceApp.Core.Importers
{
    public class HolidaysImporter : ImporterBase
    {
        private HttpClient _client;

        private HttpClientHandler _handler;

        private Regex _dateRegex = new("[0-9]{1,2}/[0-9]{1,2}/[0-9]{2}");

        public HolidaysImporter(FinanceContext context) : base(context)
        {

        }

        public async Task GetHolidays()
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            int year = 2001;
            DateTime dateUpdate = DateTime.Now;
            await DeleteAllValues();

            List<Holiday> holidays = new();
            while (true)
            {
                try
                {
                    string url = $"https://www.anbima.com.br/feriados/fer_nacionais/{year}.asp";

                    var response = await _client.GetAsync(url);

                    if (response.StatusCode != HttpStatusCode.OK)
                        break;

                    string responseString = await response.Content.ReadAsStringAsync();

                    var listMatches = _dateRegex.Matches(responseString);

                    foreach (Match match in listMatches)
                    {
                        var newHoliday = new Holiday
                        {
                            Date = DateTime.ParseExact(match.Value.ToString(), "d/M/yy", CultureInfo.InvariantCulture),
                            DateLastUpdate = dateUpdate
                        };
                        //o ParseExact converte ano de 2 digitos apenas para para anos anteriores a 2029
                        if (newHoliday.Date.Year < 2000)
                            newHoliday.Date = new DateTime(newHoliday.Date.Year + 100, newHoliday.Date.Month, newHoliday.Date.Day);

                        holidays.Add(newHoliday);
                    }


                    year++;
                }
                catch (Exception ex)
                {
                    break;
                }
            }

            //Remove duplicatas
            holidays = holidays.GroupBy(x => x.Date).Select(y => y.First()).ToList();

            await _context.Holidays.AddRangeAsync(holidays);

            
            await _context.SaveChangesAsync();
        }

        private async Task DeleteAllValues()
        {
         
            var data = _context.Holidays.ToList();

            _context.Holidays.RemoveRange(data);
            
        }
    }
}
