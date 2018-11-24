using CSharp8583.Attributes;
using System;

namespace CSharp8583.Exceptions
{
    /// <summary>
    /// Indicates the Error During the Parsing of the field
    /// </summary>
    [Serializable]
    public class ParseTagException : Exception
    {
        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="tagAttr">Tag Attribute Data</param>
        /// <param name="exMessage">error message</param>
        public ParseTagException(TagAttribute tagAttr, string exMessage) : base(exMessage)
        {
            TagData = tagAttr;
        }

        /// <summary>
        /// Contructor of Exception
        /// </summary>
        /// <param name="tagAttr">Tag Attribute Data</param>
        /// <param name="innerEx">Inner exception details</param>
        /// <param name="exMessage">error message</param>
        public ParseTagException(TagAttribute tagAttr, string exMessage, Exception innerEx) : base(exMessage, innerEx)
        {
            TagData = tagAttr;
        }

        /// <summary>
        /// Iso Field Data
        /// </summary>
        public TagAttribute TagData { get; }
    }
}

