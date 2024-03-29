﻿using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.Income
{
    public class IncomeDto : StandardDto
    {
        public string Name { get; set; }
        public ERecurrence Recurrence { get; set; }
        public string RecurrenceDisplayValue => EnumHelper<ERecurrence>.GetDisplayValue(Recurrence);
        public double Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int TimesRecurrence { get; set; }

    }
}