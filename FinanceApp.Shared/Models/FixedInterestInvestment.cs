using FinanceApp.Shared.Enum;
using System.ComponentModel.DataAnnotations;
using UsuariosApi.Models;

namespace FinanceApp.EntityFramework.Auth
{
    public class FixedInterestInvestment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name {get;set;}
        [Required]
        public ETypeFixedInterestInvestment Type {get;set;}
        [Required]
        public decimal Amount {get;set;} 
        public EIndex Index {get;set;} 
        public bool PreFixedInvestment {get;set;} 
        public decimal IndexPercentage {get;set;} 
        public decimal AdditionalFixedInterest {get;set;} 
        public DateTime InvestmentDate {get;set;} 
        public DateTime ExpirationDate {get;set;} 
        public bool LiquidityOnExpiration { get; set; }  
        public int UserId { get; set;}
        public CustomIdentityUser User { get; set; }
    }
}