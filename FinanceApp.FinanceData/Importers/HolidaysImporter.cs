﻿using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.FinanceData.Importers.Base;
using FinanceApp.EntityFramework.Data;

namespace FinanceApp.FinanceData.Importers
{
    public class HolidaysImporter : ImporterBase, IHolidaysImporter
    {
        private HttpClient _client;

        private HttpClientHandler _handler;

        private Regex _dateRegex = new("[0-9]{1,2}/[0-9]{1,2}/[0-9]{2}");

        public HolidaysImporter(FinanceDataContext context) : base(context)
        {

        }

        public async Task GetHolidays(int? yearStart = null, int? yearEnd = null)
        {
            _handler = SetDefaultHttpHandler();

            _client = new HttpClient(_handler);

            int year = 2001;
            if (yearStart != null && yearStart.Value >= 2001)
                year = yearStart.Value;
            DateTime dateUpdate = DateTime.Now;
            DeleteAllValues();

            List<Holiday> holidays = new();
            while (true || yearEnd.HasValue && year >= yearEnd.Value)
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
                catch (Exception)
                {
                    break;
                }
            }

            //Remove duplicatas
            holidays = holidays.GroupBy(x => x.Date).Select(y => y.First()).ToList();

            await _context.Holidays.AddRangeAsync(holidays);


            await _context.SaveChangesAsync();
        }

        private void DeleteAllValues()
        {

            var data = _context.Holidays.ToList();

            _context.Holidays.RemoveRange(data);

        }
    }
}
