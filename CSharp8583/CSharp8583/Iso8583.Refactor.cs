using System;
using System.Linq;
using CSharp8583.Common;
using CSharp8583.Extensions;

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
        /// <param name="isoMessage">iso message object</param>
        /// <returns>an IISOMessage object</returns>
        public IIsoMessage Parse(byte[] isoMessageBytes, IIsoMessage isoMessage)
        {
            var currentPosition = 0;

            isoMessage.MTI.Value = isoMessage.MTI.GetFieldValue(isoMessageBytes, ref currentPosition);

            isoMessage.BitMap.Value = ParseBitMap(isoMessage.BitMap, isoMessageBytes, ref currentPosition);
            ParseFields(ref isoMessage, isoMessageBytes, currentPosition);
            return isoMessage;
        }

        /// <summary>
        /// Build byte from IIsoMessage
        /// </summary>
        /// <param name="message">IIsoMessage instance</param>
        /// <returns>byte array of builded message</returns>
        public byte[] Build(IIsoMessage message) => throw new NotImplementedException();

        /// <summary>
        /// Parse Fields and Assign Values according to BitMap Value
        /// </summary>
        /// <param name="isoMessage">Iso message instance</param>
        /// <param name="isoMessageBytes">bytes of received Message</param>
        /// <param name="startPosition">position of byte Array message</param>
        private void ParseFields(ref IIsoMessage isoMessage, byte[] isoMessageBytes, int startPosition)
        {
            var currentPos = startPosition;
            //Get BitMap Fields List Values
            var bitMapFields = isoMessage.BitMap.Value.ToBinaryStringFromHex().ToList();

            for (var bitPosition = 1; bitPosition <= bitMapFields.Count - 1; bitPosition++) //TODO: Add Tags parsing
            {
                var bitMapValue = bitMapFields[bitPosition];

                //Then Field at BitPosition exists and Should Be Parsed
                if (bitMapValue == '1')
                {
                    IIsoFieldProperties fieldProperties = isoMessage.GetFieldByPosition(bitPosition + 1);

                    if(fieldProperties != null)
                    {
                        var fieldValue = fieldProperties.GetFieldValue(isoMessageBytes, ref currentPos);
                        isoMessage.SetFieldValue(bitPosition + 1, fieldValue);
                    }
                }
            }
        }

    }
}
