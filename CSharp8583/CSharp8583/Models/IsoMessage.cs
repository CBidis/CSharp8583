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
        /// Message type indicator of IsoMessage
        /// </summary>
        public string MTI { get; set; }

        /// <summary>
        /// Collection of ISO Fields
        /// </summary>
        public List<IsoField> IsoFields { get; set; }
    }
}
