using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core
{
    public class SpendingForecast
    {
        private readonly IMapper _mapper;
        private readonly FinanceContext _context;

        public SpendingForecast(IMapper mapper, FinanceContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task GetMonthlyForecast(List<DateTime> dates, CustomIdentityUser user)
        {

            var spendingsSpreadList = await GetSpendingsSpreadList(dates.Max(), user);


            //Agrupar valores


        }



        public async Task<List<SpendingSpread>> GetSpendingsSpreadList(DateTime maxYearMonth, CustomIdentityUser user)
        {
            var spendings = await _context.Spendings.AsNoTracking().ToListAsync();

            maxYearMonth = new DateTime(maxYearMonth.Year, maxYearMonth.Month, 1).AddMonths(1).AddDays(-1);
            
            var spendingsDto = _mapper.Map<List<SpendingDto>>(spendings);


            var spendingSpreadList = new List<SpendingSpread>();

            foreach (var spendingDto in spendingsDto)
            {

                if (spendingDto.Recurrence == ERecurrence.Once)
                {
                    SpendingSpread spendingSpread = _mapper.Map<SpendingSpread>(spendingDto);
                    spendingSpread.Date = spendingDto.InitialDate;

                    spendingSpreadList.Add(spendingSpread);

                }
                else
                {

                    int daysSpanTime = 0;
                    int monthsSpanTime = 0;
                    int yearSpanTime = 0;
                    DateTime? endDate = spendingDto.EndDate;

                    if (new List<ERecurrence> { ERecurrence.Daily, ERecurrence.Yearly, ERecurrence.Weekly, ERecurrence.Monthly }.Contains(spendingDto.Recurrence))
                    {
                        if (spendingDto.Recurrence == ERecurrence.Daily)
                        {
                            daysSpanTime = 1;
                        }
                        else if (spendingDto.Recurrence == ERecurrence.Weekly)
                        {
                            daysSpanTime = 7;
                        }
                        else if (spendingDto.Recurrence == ERecurrence.Monthly)
                        {
                            monthsSpanTime = 1;
                            //Para evitar problemas caso seja dia 30, irá falhar no dia 28 de feveireiro, por exemplo
                            spendingDto.InitialDate = new DateTime(spendingDto.InitialDate.Year, spendingDto.InitialDate.Month, 1);
                        }
                        else if (spendingDto.Recurrence == ERecurrence.Yearly)
                        {
                            yearSpanTime = 1;
                        }


                        if (spendingDto.TimesRecurrence > 0)
                        {
                            int years = (yearSpanTime > 0 ? yearSpanTime * (spendingDto.TimesRecurrence - 1) : 0);
                            int months = (monthsSpanTime > 0 ? monthsSpanTime * (spendingDto.TimesRecurrence - 1) : 0);
                            int days = (daysSpanTime > 0 ? daysSpanTime * (spendingDto.TimesRecurrence - 1) : 0);

                            endDate = spendingDto.InitialDate.AddDays(days).AddMonths(months).AddYears(years);

                        }
                        else if (spendingDto.IsEndless)
                        {
                            endDate = maxYearMonth;
                        }
                        DateTime initialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1); 
                        endDate = endDate < maxYearMonth ? endDate: maxYearMonth;

                        for (DateTime date = initialDate; date <= endDate; date = date.AddDays(daysSpanTime).AddMonths(monthsSpanTime).AddYears(yearSpanTime))
                        {
                            if (date < DateTime.Now.Date)
                                continue;
                            SpendingSpread spendingSpread = _mapper.Map<SpendingSpread>(spendingDto);
                            spendingSpread.Date = date;

                            spendingSpreadList.Add(spendingSpread);
                        }

                    }
                }

            }


            //        listIncomeAndSpending.forEach((element) {        
            //    //Diaria e Semanal
            //    else if ([2, 3, 4, 5].contains(element.recurrenceId)) {
            //      int daysSpanTime = 0;
            //    int monthsSpanTime = 0;
            //    int yearSpanTime = 0;
            //      //Diario
            //      if (element.recurrenceId == 2)
            //        daysSpanTime = 1;
            //      //Semanal
            //      else if (element.recurrenceId == 3)
            //        daysSpanTime = 7;
            //      //Mensal
            //      else if (element.recurrenceId == 4) {
            //        monthsSpanTime = 1;
            //        element.initialDate =
            //            DateTime(element.initialDate.year, element.initialDate.month, 1);
            //}
            //      //Anual
            //      else if (element.recurrenceId == 5) yearSpanTime = 1;

            //DateTime endDate;
            //if (element.timesRecurrence > 0)
            //    endDate = DateTime(
            //        element.initialDate.year +
            //            (yearSpanTime > 0
            //                ? yearSpanTime * (element.timesRecurrence - 1)
            //                : 0),
            //        element.initialDate.month +
            //            (monthsSpanTime > 0
            //                ? monthsSpanTime * (element.timesRecurrence - 1)
            //                : 0),
            //        element.initialDate.day +
            //            (daysSpanTime > 0
            //                ? daysSpanTime * (element.timesRecurrence - 1)
            //                : 0));
            //
            //else
            //    endDate = element.endDate;

            //for (DateTime date = element.initialDate;
            //    date.compareTo(endDate) <= 0;
            //    date = DateTime(date.year + yearSpanTime, date.month + monthsSpanTime,
            //        date.day + daysSpanTime))
            //{
            //    element.date = date;
            //    resultOpenByRecurrence.add(SpendingAndIncome.copyWith(element));
            //}
            //    }
            //  });
            return spendingSpreadList;

        }
    }
}
