﻿using FinanceApp.Shared.Enum;
using FinanceApp.Shared.Models.UserTables.Bases;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models.UserTables
{
    public class Spending : UserTable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ERecurrence Recurrence { get; set; }
        public decimal Amount { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndless { get; set; }
        public int? TimesRecurrence { get; set; }
        public bool IsRequired { get; set; }
        public Category? Category { get; set; }
        public EPayment Payment { get; set; }
        public CreditCard? CreditCard { get; set; } // Cartão de Crédito
    }
}
