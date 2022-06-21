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

        //public static T Parse(string value)
        //{
        //    return (T)Enum.Parse(typeof(T), value, true);
        //}

        //public static IList<string> GetNames(Enum value)
        //{
        //    return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        //}

        //public static IList<string> GetDisplayValues(Enum value)
        //{
        //    return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        //}

        //public static (string label, string description) GetDebitClassificationDescription(T value)
        //{
        //    var fieldInfo = value.GetType().GetField(value.ToString());

        //    var descriptionAttributes = fieldInfo.GetCustomAttributes(
        //        typeof(DocumentTypeClassificationAttribute), false) as DocumentTypeClassificationAttribute[];

        //    if (descriptionAttributes == null) return (string.Empty, string.Empty);
        //    return (descriptionAttributes.Length > 0) ? (EnumHelper<EDebitDocumentTypeClassification>.GetDisplayValue(descriptionAttributes[0].Classification), EnumHelper<EDebitDocumentTypeClassification>.GetDescriptionValue(descriptionAttributes[0].Classification)) : (value.ToString(), "");
        //}

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

            if (taxAttribute == null || taxAttribute.Length > 0) return ETypeInvestmentTax.DefaultInvestmentTax;
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

        //public static EDebitDocumentTypeClassification GetClassificationTypeValue(T value)
        //{
        //    var fieldInfo = value.GetType().GetField(value.ToString());

        //    var descriptionAttributes = fieldInfo.GetCustomAttributes(
        //        typeof(DocumentTypeClassificationAttribute), false) as DocumentTypeClassificationAttribute[];

        //    return descriptionAttributes[0].Classification;

        //}
    }
}
