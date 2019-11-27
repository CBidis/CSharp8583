using System.Collections.Generic;
using System.Linq;
using CSharp8583.Common;

namespace CSharp8583.Models
{
    /// <summary>
    /// ISO Message Model Representation
    /// </summary>
    public class IsoMessage : IIsoMessage
    {
        /// <summary>
        /// ISO Message Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Message type indicator Iso Field
        /// </summary>
        public virtual IIsoFieldProperties MTI { get; set; } = new IsoField
        {
            Position = IsoFields.MTI,
            MaxLen = 4,
            LengthType = LengthType.FIXED,
            ContentType = ContentType.B,
            DataType = DataType.ASCII
        };

        /// <summary>
        /// Bit Map Iso Field
        /// </summary>
        public virtual IIsoFieldProperties BitMap { get; set; } = new IsoField
        {
            Position = IsoFields.BitMap,
            MaxLen = 8,
            LengthType = LengthType.FIXED,
            ContentType = ContentType.B,
            DataType = DataType.HEX
        };

        /// <summary>
        /// Collection of ISO Fields
        /// </summary>
        public List<IsoField> IsoFieldsCollection { get; set; }

        /// <summary>
        /// Get Field Properties Of Iso Message
        /// </summary>
        /// <param name="position">position of the field</param>
        /// <returns>Iso Field Instance or null for non existent field</returns>
        public IIsoFieldProperties GetFieldByPosition(int position) => IsoFieldsCollection?.FirstOrDefault(p => (int)p.Position == position);

        /// <summary>
        /// Updates Iso Field Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <param name="value">value of field to set</param>
        public virtual void SetFieldValue(int position, string value)
        {
            IIsoFieldProperties fieldForUpdate = GetFieldByPosition(position);

            if (fieldForUpdate != null)
                fieldForUpdate.Value = value;
        }

        /// <summary>
        /// Updates Iso Field Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <returns>Field Value</returns>
        public virtual string GetFieldValue(int position)
        {
            IIsoFieldProperties field = GetFieldByPosition(position);
            return field?.Value; 
        }

        /// <summary>
        /// Updates Iso Field Tag Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <param name="tagName">tag name of Tag field</param>
        /// <param name="value">value of tag to set</param>
        public virtual void SetTagValue(int position, string tagName, string value)
        {
            IIsoFieldProperties fieldProperties = GetFieldByPosition(position);

            if (fieldProperties != null)
            {
                if (fieldProperties is IsoField isoField && isoField.Tags != null)
                {
                    Tag tagField = isoField.Tags.FirstOrDefault(p => p.TagName == tagName);
                    if (tagField != null)
                        tagField.Value = value;
                }
            }
        }

        /// <summary>
        /// Gets Iso Field Tag Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <param name="tagName">tag name of Tag field</param>
        public virtual string GetTagValue(int position, string tagName)
        {
            IIsoFieldProperties fieldProperties = GetFieldByPosition(position);

            if (fieldProperties != null)
            {
                if (fieldProperties is IsoField isoField && isoField.Tags != null)
                {
                    Tag tag = isoField.Tags.FirstOrDefault(p => p.TagName == tagName);
                    return tag?.Value;
                }
            }

            return null;
        }
    }
}
