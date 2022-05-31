using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}