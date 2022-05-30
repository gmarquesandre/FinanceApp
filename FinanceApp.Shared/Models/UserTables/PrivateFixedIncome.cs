﻿using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Models
{
    public class PrivateFixedIncome : UserTable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public ETypePrivateFixedIncome Type { get; set; }
        [Required]        
        public decimal Amount { get; set; }
        [Required]
        public EIndex Index { get; set; }
        [Required]
        public bool PreFixedInvestment { get; set; }
        [Required]
        public decimal IndexPercentage { get; set; }
        [Required]
        public decimal AdditionalFixedInterest { get; set; }
        [Required]
        public DateTime InvestmentDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public bool LiquidityOnExpiration { get; set; }

    }
}