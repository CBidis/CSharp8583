using CSharp8583.Attributes;
using System;
using System.Reflection;
namespace CSharp8583.Extensions
{
    /// <summary>
    /// Contains Enum Extensions
    /// </summary>
    internal static class EnumExtensions
    {
        /// <summary>
        /// Get Iso Value of Enum
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>description value</returns>
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value?.GetType().GetField(value.ToString());
            EnumIsoValueAttribute[] attributes = null;
            if (fi != null)
            {
                attributes =
                    (EnumIsoValueAttribute[])fi.GetCustomAttributes(
                    typeof(EnumIsoValueAttribute),
                    false);
            }
            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Value;
            else
                return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// retrieve enum value from EnumIsoValueAttribute
        /// </summary>
        /// <param name="enumType">enum Type</param>
        /// <param name="description">description value</param>
        /// <returns>enum value</returns>
        public static int GetValueFromDescription(this Type enumType, string description)
        {
            if (!enumType.IsEnum) throw new InvalidOperationException();

            foreach (FieldInfo field in enumType.GetFields())
            {
                if (!(Attribute.GetCustomAttribute(field, typeof(EnumIsoValueAttribute)) is EnumIsoValueAttribute attribute))
                    continue;
                if (attribute.Value == description?.Trim())
                {
                    return (int)field.GetValue(null);
                }
            }

            throw new ArgumentException($"Type {enumType.Name} does not contain a EnumIsoValueAttribute", "EnumIsoValueAttribute");
        }
    }
}
