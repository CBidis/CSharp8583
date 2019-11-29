using CSharp8583.Common;
using System;
using System.Text.RegularExpressions;

namespace CSharp8583
{
    /// <summary>
    /// Default Implementation of IValidator
    /// </summary>
    public class FieldValidator : IValidator
    {
        private readonly Regex _regexNumeric;
        private readonly Regex _regexAlphaNumeric;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FieldValidator()
        {
            _regexNumeric = new Regex(NumericPattern);
            _regexAlphaNumeric = new Regex(AlphaNumericPattern);
        }

        /// <summary>
        /// Numeric Regex Pattern (^[0-9]+$)
        /// </summary>
        protected virtual string NumericPattern { get; set; } = "^[0-9]+$";

        /// <summary>
        /// Alpha - Numeric Regex Pattern (^[A-Za-z0-9-_\\s]*$)
        /// </summary>
        protected virtual string AlphaNumericPattern { get; set; } = @"^[A-Za-z0-9-_\\s]*$";

        /// <summary>
        /// Validate Iso Field Content or Throws an Invalid Format Exception
        /// </summary>
        /// <param name="fieldProperties">iso field properties</param>
        public virtual void EnsureContent(IIsoFieldProperties fieldProperties)
        {
            if (fieldProperties.Value != null)
            {
                switch (fieldProperties.ContentType)
                {
                    case ContentType.N:
                        if (!_regexNumeric.IsMatch(fieldProperties.Value))
                            throw new FormatException($"{fieldProperties.Position} = {fieldProperties.Value} does not have a numeric value");
                        break;
                    case ContentType.ANS:
                    case ContentType.AN:
                        if (!_regexAlphaNumeric.IsMatch(fieldProperties.Value))
                            throw new FormatException($"{fieldProperties.Position} = {fieldProperties.Value} does not have a alpha numeric values");
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Validates Iso Field Len according to properties of Field or Throws an Invalid Format Exception
        /// </summary>
        /// <param name="fieldProperties">iso field properties</param>
        public virtual void EnsureLength(IIsoFieldProperties fieldProperties)
        {
            if(fieldProperties.LengthType == LengthType.FIXED)
            {
                if (fieldProperties.Value?.Length != fieldProperties.MaxLen)
                    throw new FormatException($"{fieldProperties.Position} has to be exactly {fieldProperties.MaxLen} chars <> {fieldProperties.Value}");
            }
            else
            {
                if (fieldProperties.Value?.Length <= fieldProperties.MaxLen)
                    throw new FormatException($"{fieldProperties.Position} can be at maximum {fieldProperties.MaxLen} chars < {fieldProperties.Value}");
            }
        }
    }
}
