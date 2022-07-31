namespace FinanceApp.FinanceData.Importers
{
    public interface IHolidaysImporter
    {
        Task GetHolidays(int? yearStart = null, int? yearEnd = null);
    }
}