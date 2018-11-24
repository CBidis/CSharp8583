using CSharp8583.Attributes;
using System;

namespace CSharp8583.Exceptions
{
    /// <summary>
    /// Indicates the Error During the Parsing of the field
    /// </summary>
    [Serializable]
    public class ParseFieldException : Exception
    {
        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="isoFieldAttr">iso field Attribute</param>
        /// <param name="exMessage">error message</param>
        public ParseFieldException(IsoFieldAttribute isoFieldAttr, string exMessage) : base(exMessage)
        {
            IsoFieldData = isoFieldAttr;
        }

        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="isoFieldAttr">iso field Attribute</param>
        /// <param name="innerEx">Inner exception details</param>
        /// <param name="exMessage">error message</param>
        public ParseFieldException(IsoFieldAttribute isoFieldAttr, string exMessage, Exception innerEx) : base(exMessage, innerEx)
        {
            IsoFieldData = isoFieldAttr;
        }

        /// <summary>
        /// Iso Field Data
        /// </summary>
        public IsoFieldAttribute IsoFieldData { get; }
    }
}
