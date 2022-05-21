//using FinancialApi.WebAPI.Data;
//using FinancialAPI.Data;
//using System.Globalization;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace FinanceApp.Core.Importers
//{
//    public class InvestmentFundImporter
//    {
//        private FinanceContext _context = new ();

//        public static readonly string BaseUrl = "http://dados.cvm.gov.br";

//        public async Task ImportFileAsync(string fileName)
//        {
//            Console.WriteLine($"Arquivo {fileName}");

//            string itens = await GetFileString(fileName);

//            List<InvestmentFundValue> fundValueList = ConvertToList(itens);

//            AddList(fundValueList);

//        }
//        public void AddList(List<InvestmentFundValue> list)
//        {

//            _context.AddRange(list);

//            _context.SaveChanges();
//        }


//        public List<InvestmentFundValue> ConvertToList(string str)
//        {

//            var itens = str.Replace("\r", "").Split("\n").Select(a => a.Split(";"))
//                .ToList();


//            List<string> header = itens.FirstOrDefault().Select(a => a.ToUpper()).ToList();

//            int cnpjIndex = header.IndexOf("CNPJ_FUNDO");
//            int dtComptcIndex = header.IndexOf("DT_COMPTC");
//            int vlTotalIndex = header.IndexOf("VL_TOTAL");
//            int vlQuotaIndex = header.IndexOf("VL_QUOTA");
//            int vlPatrimLiquidIndex = header.IndexOf("VL_PATRIM_LIQ");
//            int captcDiaIndex = header.IndexOf("CAPTC_DIA");
//            int resgDiaIndex = header.IndexOf("RESG_DIA");
//            int nrCotstIndex = header.IndexOf("NR_COTST");
//            int denomSocial = header.IndexOf("DENOM_SOCIAL");

//            List<int> indexes = new()
//            {
//                cnpjIndex,
//                dtComptcIndex,
//                vlTotalIndex,
//                vlQuotaIndex,
//                vlPatrimLiquidIndex,
//                captcDiaIndex,
//                resgDiaIndex,
//                nrCotstIndex,
//                denomSocial
//            };

//            if (indexes.Any(a => a == -1))
//                throw new Exception("Column not found");

//            List<InvestmentFundValue> fundValueList = new();

//            fundValueList = itens
//                //skip header
//                .Skip(1)
//                //ignore empty itens
//                .Where(item => item[0] != "")
//            .Select(a => new InvestmentFundValue()
//            {
//                CNPJ = FormatCNPJ(a[cnpjIndex]),
//                Name = a[denomSocial],
//                UnitPrice = Convert.ToDouble(a[vlQuotaIndex], CultureInfo.InvariantCulture),
                
                
//            }).ToList();

//            if (fundValueList.Any(a => ValidaCnpj(a.CNPJ) == false))
//                throw new Exception($"CNPJ is Invalid");

//            if (fundValueList.Any(a => a.Date.DayOfWeek == DayOfWeek.Saturday || a.Date.DayOfWeek == DayOfWeek.Sunday))
//                throw new Exception("Date can't be weekend");

//            if (fundValueList.Any(a => a.DayWithdraw < 0))
//                throw new Exception("withdraw value cannot be ");

//            if (fundValueList.Any(a => a.DayInvestment < 0))
//                throw new Exception("Valor do investimento não pode ser menor do que zero");

//            if (fundValueList.Any(a => a.ShareHolderCount < 0))
//                throw new Exception("Share Holders Count can't be negative");

//            return fundValueList;
//        }

//        public static string FormatCNPJ(string CNPJ)
//        {
//            CNPJ = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");

//            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
//        }

//        public bool ValidaCnpj(string cnpj)
//        {

//            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

//            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

//            int soma;

//            int resto;

//            string digito;

//            string tempCnpj;

//            cnpj = cnpj.Trim();

//            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

//            if (cnpj.Length != 14)

//                return false;

//            tempCnpj = cnpj.Substring(0, 12);

//            soma = 0;

//            for (int i = 0; i < 12; i++)

//                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

//            resto = (soma % 11);

//            if (resto < 2)

//                resto = 0;

//            else

//                resto = 11 - resto;

//            digito = resto.ToString();

//            tempCnpj = tempCnpj + digito;

//            soma = 0;

//            for (int i = 0; i < 13; i++)

//                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

//            resto = (soma % 11);

//            if (resto < 2)

//                resto = 0;

//            else

//                resto = 11 - resto;

//            digito = digito + resto.ToString();

//            return cnpj.EndsWith(digito);

//        }

//        public async Task<string> GetFileString(string fileName)
//        {
//            HttpClient client = new();

//            var stream = await client.GetStreamAsync($"{BaseUrl}/dados/FI/DOC/INF_DIARIO/DADOS/{fileName}");

//            byte[] bytes = StreamToByte(stream);

//            string str = Encoding.Default.GetString(bytes);

//            return str;

//        }

//        public async Task<List<string>> GetFileList()
//        {
//            HttpClient client = new();

//            var response = await client.GetAsync(requestUri: $"{BaseUrl}/dados/FI/DOC/INF_DIARIO/DADOS/");

//            string responseBody = await response.Content.ReadAsStringAsync();

//            Regex regexFileName = new("\"inf_diario_fi_(.*).csv\"");

//            var fileMatches = regexFileName.Matches(responseBody);

//            List<string> listFiles = fileMatches.Cast<Match>().Select(match => match.Value.Replace("\"", "")).ToList();

//            return listFiles;
//        }

//        public byte[] StreamToByte(Stream input)
//        {
//            using MemoryStream ms = new();
//            input.CopyTo(ms);
//            return ms.ToArray();
//        }




//    }
//}
