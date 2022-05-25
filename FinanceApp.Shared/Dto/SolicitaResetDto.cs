using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class SolicitaResetDto
    {
        [Required]
        public string Email { get; set; }
    }
}