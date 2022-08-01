using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared.Dto
{
    public class CreateUsuarioDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;
        [Required]
        public string Email { get; set; } = String.Empty;
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = String.Empty;
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string RePassword { get; set; } = String.Empty;

    }
}