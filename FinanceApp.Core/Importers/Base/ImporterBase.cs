using FinancialApi.WebAPI.Data;
using System.Globalization;
using System.Net;

namespace FinanceApp.Core.Importers
{
    public class ImporterBase
    {

        public FinanceContext _context = new();

        public CultureInfo _cultureInfoPtBr = new("pt-br");

        public static HttpClientHandler SetDefaultHttpHandler(bool useProxy = false)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = new CookieContainer();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.MaxConnectionsPerServer = 2;
            return httpClientHandler;
        }


        public byte[] StreamToByte(Stream input)
        {
            using MemoryStream ms = new();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}