using System;
using CSharp8583.Common;

namespace CSharp8583.Attributes
{
    /// <summary>
    /// Used to Decorate properties of POCOs
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Property, Inherited = false)]
    public sealed class IsoFieldAttribute : Attribute, IIsoFieldProperties
    {
        /// <summary>
        /// Common Constructor of Iso Field
        /// </summary>
        /// <param name="position">position of Field</param>
        /// <param name="maxLen">max Len of Field value</param>
        /// <param name="lengthType">length Type</param>
        /// <param name="contentType">content type</param>
        /// <param name="dataType">data type</param>
        /// <param name="lenDataType">length data type</param>
        /// <param name="encoding">Encoding type in case that the bytes are not in Common ASCII char set</param>
        public IsoFieldAttribute(IsoFields position, int maxLen, LengthType lengthType, ContentType contentType, DataType dataType = DataType.ASCII,
                                                                    DataType lenDataType = DataType.ASCII, EncodingType encoding = EncodingType.None)
        {
            Position = position;
            MaxLen = maxLen;
            LengthType = lengthType;
            ContentType = contentType;
            DataType = dataType;
            LenDataType = lenDataType;
            Encoding = encoding;
        }

        /// <summary>
        /// Value property, can be used for Default value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Iso Field Number
        /// </summary>
        public IsoFields Position { get; set; }

        /// <summary>
        /// Iso Field Number
        /// </summary>
        public int MaxLen { get; set; }

        /// <summary>
        /// Length Type of Field
        /// </summary>
        public LengthType LengthType { get; set; }

        /// <summary>
        /// Content Type of Field
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Data Type of Field
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// Len Data Type of Field
        /// </summary>
        public DataType LenDataType { get; set; }

        /// <summary>
        /// Encoding
        /// </summary>
        public EncodingType Encoding { get; set; }
    }
}
