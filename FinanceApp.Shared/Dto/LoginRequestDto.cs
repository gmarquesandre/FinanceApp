using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Requests
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}