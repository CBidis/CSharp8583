using CSharp8583.Common;
using System;

namespace CSharp8583.Exceptions
{
    /// <summary>
    /// Exception thrown in case of a Build Field Error
    /// </summary>
    [Serializable]
    public class BuildFieldException : Exception
    {
        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="isoFieldAttr">iso field Attribute</param>
        /// <param name="exMessage">error message</param>
        public BuildFieldException(IIsoFieldProperties isoFieldAttr, string exMessage) : base(exMessage)
        {
            IsoFieldData = isoFieldAttr;
        }

        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="isoFieldAttr">iso field Attribute</param>
        /// <param name="innerEx">Inner exception details</param>
        /// <param name="exMessage">error message</param>
        public BuildFieldException(IIsoFieldProperties isoFieldAttr, string exMessage, Exception innerEx) : base(exMessage, innerEx)
        {
            IsoFieldData = isoFieldAttr;
        }

        /// <summary>
        /// Iso Field Data
        /// </summary>
        public IIsoFieldProperties IsoFieldData { get; }
    }
}
