using System;
using CSharp8583.Common;
using CSharp8583.Extensions;
using CSharp8583.Models;

namespace CSharp8583
{
	/// <summary>
    /// Partial Implementation for Dynamic assign of Fields and Parsing
    /// </summary>
    public partial class Iso8583
    {
        /// <summary>
        /// Parse Iso Message bytes to IIsoMessage object
        /// </summary>
        /// <param name="isoMessageBytes">bytes to parse</param>
        /// <returns>an IISOMessage object</returns>
        public IIsoMessage Parse(byte[] isoMessageBytes)
        {
            var currentPosition = 0;
            var messageIntance = new IsoMessage();

            messageIntance.MTI.Value = messageIntance.MTI.GetFieldValue(isoMessageBytes, ref currentPosition);

            messageIntance.BitMap.Value = ParseBitMap(messageIntance.BitMap, isoMessageBytes, ref currentPosition);
            //ParseFields(ref messageIntance, isoMessageBytes, currentPosition); //TODO: Complete new implementations
            return messageIntance;
        }

        /// <summary>
        /// Build byte from IIsoMessage
        /// </summary>
        /// <param name="message">IIsoMessage instance</param>
        /// <returns>byte array of builded message</returns>
        public byte[] Build(IIsoMessage message) => throw new NotImplementedException();

    }
}
