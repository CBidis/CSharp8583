namespace CSharp8583.Common
{
    /// <summary>
    /// ISO Field Properties
    /// </summary>
    public interface IIsoFieldProperties
    {
        /// <summary>
        /// Iso Field Number
        /// </summary>
        IsoFields Position { get; set; }

        /// <summary>
        /// Iso Field Number
        /// </summary>
        int MaxLen { get; set; }

        /// <summary>
        /// Length Type of Field
        /// </summary>
        LengthType LengthType { get; set; }

        /// <summary>
        /// Content Type of Field
        /// </summary>
        ContentType ContentType { get; set; }

        /// <summary>
        /// Data Type of Field
        /// </summary>
        DataType DataType { get; set; }

        /// <summary>
        /// Len Data Type of Field
        /// </summary>
        DataType LenDataType { get; set; }

        /// <summary>
        /// Encoding
        /// </summary>
        EncodingType Encoding { get; set; }
    }
}
