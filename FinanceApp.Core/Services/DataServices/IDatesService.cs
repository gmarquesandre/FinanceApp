﻿using FinanceApp.Shared.Dto;

namespace FinanceApp.Core.Services.DataServices
{
    public interface IDatesService : IScopedService
    {
        Task<WorkingDaysByYearDto?> GetWorkingDaysOfAYear(int year);
        Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear(int yearStart, int? yearEnd);
        Task<List<WorkingDaysByYearDto>> GetWorkingDaysByYear();
        Task<List<HolidayDto>> GetHolidays();
        Task<List<HolidayDto>> GetHolidays(DateTime? startDate = null, DateTime? endDate = null);
        Task<bool> IsHoliday(DateTime date);
    }
}