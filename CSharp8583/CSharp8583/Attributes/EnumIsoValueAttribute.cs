using System;

namespace CSharp8583.Attributes
{
    /// <summary>
    /// Used as an Annotation to Enums and specifies the Value that will be used during the ISO Parsing/Building
    /// </summary>
    public class EnumIsoValueAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">value for the Iso field</param>
        public EnumIsoValueAttribute(string value) => Value = value;

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; }
    }
}
