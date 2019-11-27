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
        public List<IIsoFieldProperties> IsoFieldsCollection { get; set; }

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
        public void SetFieldValue(int position, string value)
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
        public string GetFieldValue(int position)
        {
            IIsoFieldProperties fieldForUpdate = GetFieldByPosition(position);
            return fieldForUpdate == null ? null : fieldForUpdate.Value; 
        }
    }
}
