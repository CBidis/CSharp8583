using CSharp8583.Common;

namespace CSharp8583.Models
{
    /// <summary>
    /// Tag Model Representation
    /// </summary>
    public class Tag : ITagProperties
    {
        /// <summary>
        /// Value property, can be used for Default value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Tag Position
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Tag Name Value
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Lentgh of Length Bytes
        /// </summary>
        public int LenBytesLen { get; set; }

        /// <summary>
        /// Data Type of Field
        /// </summary>
        public DataType DataType { get; set; } = DataType.ASCII;

        /// <summary>
        /// Len Data Type of Field
        /// </summary>
        public DataType LenDataType { get; set; } = DataType.ASCII;

        /// <summary>
        /// Encoding
        /// </summary>
        public EncodingType Encoding { get; set; } = EncodingType.None;

        /// <summary>
        /// IsTLV
        /// </summary>
        public bool IsTLV { get; set; } = true;

        /// <summary>
        /// Lentgh of Tag Bytes, with a default value of 2
        /// </summary>
        public int TagBytesLen { get; set; } = 2;
    }
}
