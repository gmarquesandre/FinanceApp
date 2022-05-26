﻿using FinancialApi.WebAPI.Data;
using System.Globalization;
using System.Net;

namespace FinanceApp.Core.Importers
{
    public class ImporterBase
    {

        public FinanceContext _context;

        public CultureInfo _cultureInfoPtBr = new("pt-br");

        public ImporterBase(FinanceContext context)
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