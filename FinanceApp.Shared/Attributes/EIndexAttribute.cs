using FinanceApp.Shared.Enum;

namespace FinanceApp.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IndexAttribute : Attribute
    {
        public EIndex Index;
        public IndexAttribute(EIndex index)
        {
            Index = index;
        }

    }
}
