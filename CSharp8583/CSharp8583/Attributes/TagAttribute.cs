using CSharp8583.Common;
using System;

namespace CSharp8583.Attributes
{
    /// <summary>
    /// Used To Decorate Properties for Reserved Fields Parsing
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Property, Inherited = false)]
    public class TagAttribute : Attribute, ITagProperties
    {
        /// <summary>
        /// Constructor of Tag Field Value
        /// </summary>
        /// <param name="position">position in TLV</param>
        /// <param name="tagName">tag value name</param>
        /// <param name="lenBytesLen">number of bytes to parse for Length</param>
        /// <param name="tagBytesLen"></param>
        /// <param name="isTlv">whether is TLV (Tag Lentgh Value) or LTV (Lentgh Tag Value)</param>
        /// <param name="dataType">datatype of Value</param>
        /// <param name="lenDataType">datatype of lentgh value</param>
        /// <param name="encoding">Encoding type in case that the bytes are not in Common ASCII char set</param>
        public TagAttribute(int position, string tagName, int lenBytesLen = 2, int tagBytesLen = 2, bool isTlv = true,  
                    DataType dataType = DataType.ASCII, DataType lenDataType = DataType.ASCII, EncodingType encoding = EncodingType.None)
        {
            Position = position;
            TagName = tagName;
            LenBytesLen = lenBytesLen;
            TagBytesLen = tagBytesLen;
            DataType = dataType;
            LenDataType = lenDataType;
            Encoding = encoding;
            IsTLV = isTlv;
        }

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
        public DataType DataType { get; set; }

        /// <summary>
        /// Len Data Type of Field
        /// </summary>
        public DataType LenDataType { get; set; }

        /// <summary>
        /// Encoding
        /// </summary>
        public EncodingType Encoding { get; set; }

        /// <summary>
        /// IsTLV
        /// </summary>
        public bool IsTLV { get; set; }

        /// <summary>
        /// Lentgh of Length Bytes
        /// </summary>
        public int TagBytesLen { get; set; }
    }
}
