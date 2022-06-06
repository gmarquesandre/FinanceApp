using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Dto
{
    public class ForecastList
    {
        public EItemType Type { get; set; }
        public string TypeDisplayValue => EnumHelper<EItemType>.GetDisplayValue(Type);
        public List<ForecastItem> Items { get; set; }
    }
}
