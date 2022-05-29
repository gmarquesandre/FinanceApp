﻿using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.Shared.Models
{
    public class FixedInterestInvestment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public ETypeFixedInterestInvestment Type { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public EIndex Index { get; set; }
        [Required]
        public bool PreFixedInvestment { get; set; }
        [Required]
        public decimal IndexPercentage { get; set; }
        public decimal? AdditionalFixedInterest { get; set; }
        [Required]
        public DateTime InvestmentDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public bool LiquidityOnExpiration { get; set; }
        [Required]
        public int UserId { get; set; }
        public CustomIdentityUser User { get; set; }
    }
}