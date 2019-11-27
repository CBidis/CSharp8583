using CSharp8583.Common;
using System.Collections.Generic;
using System.Linq;

namespace CSharp8583.Models
{
    /// <summary>
    /// ISO Field Model Representation
    /// </summary>
    public class IsoField : IIsoFieldProperties
    {
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

        /// <summary>
        /// Tags defined for field of L Types
        /// </summary>
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Set Tag Value field
        /// </summary>
        /// <param name="tagName">tag Name</param>
        /// <param name="tagValue">tag value</param>
        public virtual void SetTagValue(string tagName, string tagValue)
        {
            Tag tag = Tags.FirstOrDefault(p => p.TagName == tagName);

            if (tag != null)
                tag.Value = tagValue;
        }

        /// <summary>
        /// Get Tag Value field
        /// </summary>
        /// <param name="tagName">tag Name</param>
        public virtual string GetTagValue(string tagName)
        {
            Tag tag = Tags.FirstOrDefault(p => p.TagName == tagName);
            return tag?.Value;
        }
    }
}
