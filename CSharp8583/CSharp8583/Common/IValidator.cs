namespace CSharp8583.Common
{
    /// <summary>
    /// Field/Tags Validator
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validate Iso Field Content or Throws an Invalid Format Exception
        /// </summary>
        /// <param name="fieldProperties">iso field properties</param>
        void EnsureContent(IIsoFieldProperties fieldProperties);

        /// <summary>
        /// Validates Iso Field Len according to properties of Field or Throws an Invalid Format Exception
        /// </summary>
        /// <param name="fieldProperties">iso field properties</param>
        void EnsureLength(IIsoFieldProperties fieldProperties);
    }
}
