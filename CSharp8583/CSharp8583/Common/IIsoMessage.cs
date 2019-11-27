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

        /// <summary>
        /// Get Field Properties Of Iso Message
        /// </summary>
        /// <param name="position">position of the field</param>
        /// <returns>Iso Field Instance</returns>
        IIsoFieldProperties GetFieldByPosition(int position);

        /// <summary>
        /// Updates Iso Field Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <param name="value">value of field to set</param>
        void SetFieldValue(int position, string value);

        /// <summary>
        /// Updates Iso Field Value
        /// </summary>
        /// <param name="position">field position</param>
        /// <returns>Field Value</returns>
        string GetFieldValue(int position);
    }
}
