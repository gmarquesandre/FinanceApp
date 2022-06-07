﻿using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Models.CommonTables
{
    public class IndexValueDto
    {
        public EIndex Index { get; set; }
        public EIndexRecurrence IndexRecurrence { get; set; }
        public string IndexRecurrenceName => EnumHelper<EIndexRecurrence>.GetDisplayValue(IndexRecurrence);
        public string IndexName => EnumHelper<EIndex>.GetDisplayValue(Index);
        public DateTime Date { get; set; }
        public DateTime DateEnd { get; set; }
        public double Value { get; set; }
    }
}
