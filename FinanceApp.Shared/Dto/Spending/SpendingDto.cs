﻿using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models
{
    public class SpendingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ERecurrence Recurrence { get; set; }
        private string RecurrenceDisplayValue => EnumHelper<ERecurrence>.GetDisplayValue(Recurrence);
        public CategoryDto? Category { get; set; }
        public bool IsRequired { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int TimesRecurrence { get; set; }

    }
}