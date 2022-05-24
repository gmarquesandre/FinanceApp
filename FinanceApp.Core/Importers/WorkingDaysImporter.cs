using FinancialAPI.Data;

namespace FinanceApp.Core.Importers
{
    public class WorkingDaysImporter : ImporterBase
    {

        public HolidaysImporter holidaysImporter = new();
        public int GetWorkingDays(DateTime from, DateTime to)
        {
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }

        public async Task ImportWorkingDays()
        {


            await DeleteAllValues();

            List<DateTime> holidays = _context.Holidays.ToList().Select(a => a.Date).ToList();

            if(!holidays.Any())  
                await holidaysImporter.ImportHolidays();

            List<int> years = holidays.Select(a => a.Year).Distinct().ToList();

            List<WorkingDaysByYear> workingDaysByYearList = new();


            foreach(var year in years)
            {
                var holidaysThisYear = holidays.Where(a => a.Year == year).ToList();

                int validHolidaysYearCount = holidaysThisYear.Where(a => a.Date.DayOfWeek != DayOfWeek.Sunday && a.Date.DayOfWeek != DayOfWeek.Saturday).Count();


                var firstDay = new DateTime(year, 1, 1);
                var lastDay = new DateTime(year + 1, 1, 1).AddDays(-1);

                int workingDays = GetWorkingDays(firstDay, lastDay) - validHolidaysYearCount;

                workingDaysByYearList.Add(new()
                {
                    DateLastUpdate = DateTime.Now,
                    WorkingDays = workingDays,
                    Year = year
                });

            }

            _context.WorkingDaysByYear.AddRange(workingDaysByYearList);
            _context.SaveChanges();
        
        }

        private async Task DeleteAllValues()
        {

            var data = _context.WorkingDaysByYear.ToList();

            _context.WorkingDaysByYear.RemoveRange(data);

        }

    }
}
