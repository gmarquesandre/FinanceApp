using FinanceApp.Shared.Dto.Base;

namespace FinanceApp.Shared.Dto.CurrentBalance
{
    public class CreateOrUpdateCurrentBalance : UpdateDto
    {
        public double Value { get; set; }
        public double? PercentageCdi { get; set; }
        public bool UpdateValueWithCdiIndex { get; set; }

    }
}
