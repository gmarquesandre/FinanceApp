using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EIndexAttribute : Attribute
    {
        public EIndex Index;
        public EIndexAttribute(EIndex index)
        {
            Index = index;
        }

    }
}
