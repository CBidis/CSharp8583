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
        /// MTI Field
        /// </summary>
        IIsoFieldProperties MTI { get; set; }

        /// <summary>
        /// Bit Map Field
        /// </summary>
        IIsoFieldProperties BitMap { get; set; }

        /// <summary>
        /// Collection of ISO Fields
        /// </summary>
        List<IIsoFieldProperties> IsoFieldsCollection { get; set; }
    }
}
