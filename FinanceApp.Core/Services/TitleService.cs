using FinanceApp.Shared;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Core.Services
{
    public partial class TitleService : ITitleService
    {
        public IIndexService _indexService;

        public TitleService(IIndexService indexService)
        {
            _indexService = indexService;
        }

        public async Task<DefaultTitleOutput> GetCurrentValueOfTitle(DefaultTitleInput input)
        {
            if (input.InvestmentValue > 0.00)
            {
                var apprecitation = await _indexService.GetIndexValueBetweenDates(EIndex.CDI,
                                            input.DateInvestment,
                                            input.Date,
                                            input.IndexPercentage);

                var grossValue = Convert.ToDouble(input.InvestmentValue) * apprecitation;

                var incomeTaxPercentage = GetIncomeTax(input.TypePrivateFixedIncome, input.Date, input.DateInvestment);

                var iofPercentage = _indexService.GetIof((input.Date - input.DateInvestment).Days);

                var iofValue = iofPercentage * (grossValue - Convert.ToDouble(input.InvestmentValue));

                var grossValueAfterIof = grossValue - iofValue;

                var incomeTaxValue = incomeTaxPercentage * (grossValueAfterIof - input.InvestmentValue);

                var liquidValue = grossValueAfterIof - incomeTaxValue;

                return new DefaultTitleOutput()
                {
                    DateInvestment = input.DateInvestment,
                    Date = input.Date,
                    GrossValue = grossValue,
                    IncomeTaxValue = incomeTaxValue,
                    LiquidValue = liquidValue,
                    InvestmentValue = input.InvestmentValue,
                    IofValue = iofValue,
                    AdditionalFixedInterest = input.AdditionalFixedInterest,
                    Index = input.Index,
                    IndexPercentage = input.IndexPercentage
                };
            }
            else if (input.InvestmentValue < 0)
            {

                //Aqui poderia colocar o juros de empréstimo da conta
                return new DefaultTitleOutput()
                {
                    DateInvestment = input.DateInvestment,
                    Date = input.Date,
                    GrossValue = 0,
                    IncomeTaxValue = 0,
                    LiquidValue = 0,
                    InvestmentValue = input.InvestmentValue,
                    IofValue = 0,
                    AdditionalFixedInterest = input.AdditionalFixedInterest,
                    Index = input.Index,
                    IndexPercentage = input.IndexPercentage
                };
            }
            else
            {

                return new DefaultTitleOutput()
                {
                    DateInvestment = input.DateInvestment,
                    Date = input.Date,
                    GrossValue = 0,
                    IncomeTaxValue = 0,
                    LiquidValue = 0,
                    InvestmentValue = input.InvestmentValue,
                    IofValue = 0,
                    AdditionalFixedInterest = input.AdditionalFixedInterest,
                    Index = input.Index,
                    IndexPercentage = input.IndexPercentage
                };
            }

        }

        private double GetIncomeTax(ETypePrivateFixedIncome typePrivateFixedIncome, DateTime date, DateTime dateInvestment)
        {
            var typeTax = EnumHelper<ETypePrivateFixedIncome>.GetInvestmentTax(typePrivateFixedIncome);

            if (typeTax == ETypeInvestmentTax.NotApplied)
            {
                return 0;
            }
            else if (typeTax == ETypeInvestmentTax.BalanceTax)
            {
                return 0.225;
            }
            else if (typeTax == ETypeInvestmentTax.DefaultInvestmentTax)
            {
                //Até 180 dias  22.5%
                //Entre 181 e 360 dias 20.0
                //Entre 361 dias e 720 17.5%
                // >= 720  dias 15.0%
                if ((date - dateInvestment).Days <= 180)
                    return 0.225;
                else if ((date - dateInvestment).Days <= 360 && (date - dateInvestment).Days >= 181)
                    return 0.20;
                else if ((date - dateInvestment).Days <= 720 && (date - dateInvestment).Days >= 361)
                    return 0.20;
                else
                    return 0.15;
            }
            return 0;
        }
    }
}
