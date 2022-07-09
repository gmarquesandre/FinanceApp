using System.Security.Claims;

namespace FinanceApp.EntityFramework
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
