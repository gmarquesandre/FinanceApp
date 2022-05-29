using FinanceApp.Shared.Enum;
using System;

namespace FinanceApp.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeInvestmentTaxAttribute : Attribute
    {
        public ETypeInvestmentTax IncomeTax;
        public TypeInvestmentTaxAttribute(ETypeInvestmentTax incomeTax)
        {
            IncomeTax = incomeTax;
        }

    }
}
