using CSharp8583.Models;
using System.Collections.Generic;

namespace CSharp8583.Common
{
    /// <summary>
    /// ISO Message Properties
    /// </summary>
    public interface IIsoMessage
    {
        /// <summary>
        /// Message type indicator of IsoMessage
        /// </summary>
        string MTI { get; set; }

        /// <summary>
        /// Collection of ISO Fields
        /// </summary>
        List<IsoField> IsoFields { get; set; }
    }
}
