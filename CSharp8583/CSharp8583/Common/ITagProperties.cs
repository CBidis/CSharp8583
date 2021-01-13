namespace CSharp8583.Common
{
    /// <summary>
    /// Properties Defined per Tag Attribute
    /// </summary>
    public interface ITagProperties
    {
        /// <summary>
        /// Value property, can be used for Default value
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Tag Position
        /// </summary>
        int Position { get; set; }

        /// <summary>
        /// Tag Name Value
        /// </summary>
        string TagName { get; set; }

        /// <summary>
        /// Lentgh of Length Bytes
        /// </summary>
        int LenBytesLen { get; set; }

        /// <summary>
        /// Lentgh of Tag Bytes
        /// </summary>
        int TagBytesLen { get; set; }

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

        /// <summary>
        /// IsTLV
        /// </summary>
        bool IsTLV { get; set; }
    }
}
