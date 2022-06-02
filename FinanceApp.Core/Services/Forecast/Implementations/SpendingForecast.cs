using AutoMapper;
using FinanceApp.Core.Services.CrudServices.Interfaces;
using FinanceApp.EntityFramework;
using FinanceApp.Shared.Dto.CreditCard;
using FinanceApp.Shared.Dto.Spending;
using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.CommonTables;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Core.Services.Forecast.Implementations
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
            var spendings = await _context.Spendings.Include(a=> a.CreditCard).AsNoTracking().ToListAsync();

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
                    }
                    else if (spendingDto.Recurrence == ERecurrence.Yearly)
                    {
                        yearSpanTime = 1;
                    }


                    if (spendingDto.TimesRecurrence > 0)
                    {
                        int years = yearSpanTime > 0 ? yearSpanTime * (spendingDto.TimesRecurrence - 1) : 0;
                        int months = monthsSpanTime > 0 ? monthsSpanTime * (spendingDto.TimesRecurrence - 1) : 0;
                        int days = daysSpanTime > 0 ? daysSpanTime * (spendingDto.TimesRecurrence - 1) : 0;

                        endDate = spendingDto.InitialDate.AddDays(days).AddMonths(months).AddYears(years);

                    }
                    else if (spendingDto.IsEndless)
                    {
                        endDate = maxYearMonth;
                    }

                    endDate = endDate < maxYearMonth ? endDate : maxYearMonth;

                    DateTime date = spendingDto.InitialDate;

                    while (date <= endDate)
                    {
                        //ignora datas passadas
                        if (date >= DateTime.Now.Date)
                        {
                            SpendingSpread spendingSpread = _mapper.Map<SpendingSpread>(spendingDto);
                            if (spendingDto.Payment == EPayment.Cash)
                            {
                                spendingSpread.Date = date;
                                spendingSpread.ReferenceDate = date;

                            }
                            else if(spendingDto.Payment == EPayment.Credit)
                            {
                                spendingSpread.ReferenceDate = CheckDateForCredit(date, spendingDto.CreditCard!);

                                spendingSpread.Date = date;
                            }

                            spendingSpreadList.Add(spendingSpread);
                        }


                        try
                        {
                            date = date.AddDays(daysSpanTime).AddMonths(monthsSpanTime).AddYears(yearSpanTime);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            //if date written is like 31 february, does not exist, it will use last day of month
                            date = new DateTime(date.Year, +date.Month, 1).AddDays(-1);

                        }
                    }
                }

            }
            return spendingSpreadList;

        }

        private DateTime CheckDateForCredit(DateTime date, CreditCardDto creditCard)
        {


            if (date.Day >= creditCard.InvoiceClosingDay)
            {
                return new DateTime(date.Year, date.Month + 1, creditCard.InvoicePaymentDay);
            }
            else
            {
                return new DateTime(date.Year, date.Month, creditCard.InvoicePaymentDay);
            }
        }
    }
}
