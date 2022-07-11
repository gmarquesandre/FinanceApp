using FinanceApp.Shared.Dto.Base;
using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto.TreasuryBond
{
    public class TreasuryBondDto : StandardDto
    {
        public ETreasuryBond Type { get; set; }
        public string TypeDisplayValue => EnumHelper<ETreasuryBond>.GetDisplayValue(Type);
        public double UnitPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public EOperation Operation { get; set; }
        public string OperationName => EnumHelper<EOperation>.GetDisplayValue(Operation);
        public string OperationNameShort => EnumHelper<EOperation>.GetDisplayShortValue(Operation);
        public double Quantity { get; set; }

    }
}