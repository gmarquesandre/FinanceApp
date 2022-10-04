using FinanceApp.Shared.Attributes;
using FinanceApp.Shared.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Shared
{
    public static class EnumHelper<T>
    {
        public static T GetValueFromName(string name)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name.ToLower() == name.ToLower())
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException(name);
        }        

        public static int GetOrderValue(T value)
        {

            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return int.MaxValue;
            return descriptionAttributes.Length > 0 ?
                    descriptionAttributes[0].GetOrder() ?? 0
                    : int.MaxValue;
        }

        public static string GetDescriptionValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];



            if (descriptionAttributes == null) return string.Empty;
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : value.ToString();
        }



        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];

                if (descriptionAttributes == null) return string.Empty;
                return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Name : value.ToString();
            }

            return value.ToString();
        }
        
        public static ETypeInvestmentTax GetInvestmentTax(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var taxAttribute = fieldInfo.GetCustomAttributes(
                typeof(TypeInvestmentTaxAttribute), false) as TypeInvestmentTaxAttribute[];

            if (taxAttribute == null || taxAttribute.Length == 0) return ETypeInvestmentTax.DefaultInvestmentTax;
            return taxAttribute[0].IncomeTax;
        }


        public static string GetDisplayShortValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo != null)
            {
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];

                if (descriptionAttributes == null) return string.Empty;
                return descriptionAttributes.Length > 0 ? descriptionAttributes[0].ShortName : value.ToString();
            }

            return value.ToString();
        }
    }
}
