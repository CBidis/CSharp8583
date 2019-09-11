using CSharp8583.Extensions;
using System;
using System.Reflection;

namespace CSharp8583.Common
{
    /// <summary>
    /// Contains Shared Utils Methods
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Validates the Underlyning Type and Sets the Corresponding Type
        /// </summary>
        /// <param name="instance">instance to set values</param>
        /// <param name="prop">prop info of Instance</param>
        /// <param name="value">value to set</param>
        internal static void SetValueToInstance(object instance, PropertyInfo prop, string value)
        {
            if (prop.PropertyType.IsEnum)
                prop.SetValue(instance, prop.PropertyType.GetValueFromDescription(value), null);
            else
                prop.SetValue(instance, value, null);
        }

        /// <summary>
        /// Get the object value for the ISO Field
        /// </summary>
        /// <param name="instance">instance to get value</param>
        /// <param name="prop">prop info of Instance</param>
        /// <returns>value for ISO  Field</returns>
        internal static object GetValueFromInstance(object instance, PropertyInfo prop)
        {
            if (prop.PropertyType.IsEnum)
                return ((Enum)Enum.Parse(prop.PropertyType, prop.GetValue(instance, null).ToString()))?.GetEnumDescription() ?? string.Empty;
            else
                return prop.GetValue(instance, null);
        }
    }
}
