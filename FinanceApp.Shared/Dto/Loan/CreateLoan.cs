﻿using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Models
{
    public class CreateLoan : CreateDto
    {
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public decimal LoanValue { get; set; }
        public decimal InterestRate { get; set; }
        public int Type { get; set; }

    }
}
