using FinanceApp.EntityFramework.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Net;

namespace FinanceApp.FinanceData.Importers.Base
{
    public class ImporterBase
    {

        public FinanceDataContext _context;
        public IMemoryCache _memoryCache;

        public CultureInfo _cultureInfoPtBr = new("pt-br");

        public CultureInfo _cultureInvariant = CultureInfo.InvariantCulture;


        public ImporterBase(FinanceDataContext context)
        {
            _context = context;
        }

        public static HttpClientHandler SetDefaultHttpHandler(bool useProxy = false)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = new CookieContainer();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.MaxConnectionsPerServer = 2;
            return httpClientHandler;
        }

        public static int CheckIfFound(int value)
        {
            if (value < 0)
                throw new Exception("Indice não encontrado");
            return value;
        }

        public byte[] StreamToByte(Stream input)
        {
            using MemoryStream ms = new();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}