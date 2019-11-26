using System.Collections.Generic;
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
    }
}
