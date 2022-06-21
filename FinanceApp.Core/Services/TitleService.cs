using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services.ForecastServices
{
    public class TitleService
    {
        public IIndexService _indexService;

        public TitleService(IIndexService indexService)
        {
            _indexService = indexService;
        }

        public class DefaultTitleOutput
        {
            public DateTime Date { get; set; }
            public double GrossValue { get; set; }
            public double LiquidValue { get; set; }
            public double IofValue { get; set; }
            public double IncomeTaxValue { get; set; }
        }

        public class DefaultTitleInput
        {
            public DateTime DateInvestment { get; set; }
            public DateTime Date { get; set; }
            public double InvestmentValue { get; set; }
            public EIndex Index { get; set; }
            public double IndexPercentage { get; set; }
            public double AdditionalFixedInterest { get; set; }

        }


        public async Task<(double grossValue, double liquidValue, double iof, double incomeTax)>
            GetCurrentValueOfTitle(DateTime dateInvestment, double investmentValue, DateTime date, bool updateWithCdiIndex, double indexPercentage, bool IsBalance = true)
        {
            if (investmentValue > 0 && updateWithCdiIndex)
            {
                var apprecitation = await _indexService.GetIndexValueBetweenDates(EIndex.CDI,
                                            dateInvestment,
                                            date,
                                            indexPercentage);

                var grossValue = Convert.ToDouble(investmentValue) * apprecitation;

                var incomeTaxPercentage = 0.225;

                var iofPercentage = _indexService.GetIof((date - dateInvestment).Days);

                var iofValue = iofPercentage * (grossValue - Convert.ToDouble(investmentValue));

                var grossValueAfterIof = grossValue - iofValue;

                var incomeTaxValue = incomeTaxPercentage * (grossValueAfterIof - Convert.ToDouble(investmentValue));

                var liquidValue = grossValueAfterIof - incomeTaxValue;

                return (grossValue, liquidValue, iofValue, incomeTaxValue);
            }
            else if (investmentValue < 0)
            {

                //Aqui poderia colocar o juros de empréstimo da conta
                return (0, 0, 0, 0);
            }
            else
            {

                return (Convert.ToDouble(investmentValue), Convert.ToDouble(investmentValue), 0, 0);
            }

        }
    }
}
