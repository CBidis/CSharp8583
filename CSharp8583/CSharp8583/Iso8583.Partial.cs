using System;
using System.Linq;
using CSharp8583.Common;
using CSharp8583.Models;
using CSharp8583.Extensions;
using System.Collections.Generic;

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
            //Keep Only Fields with values,
            isoMessage.IsoFieldsCollection = isoMessage.IsoFieldsCollection.Where(p => p.Value != null).ToList();
            return isoMessage;
        }

        /// <summary>
        /// Build byte from IIsoMessage
        /// </summary>
        /// <param name="message">IIsoMessage instance</param>
        /// <returns>byte array of builded message</returns>
        public byte[] Build(IIsoMessage message)
        {
            //Get Only Fields that Have values or Tags Defined with values
            IEnumerable<int> orderedFieldPositions = message.IsoFieldsCollection
                .Where(c => c.Value != null || (c.Tags?.Count > 0 && c.Tags.Any(tag => tag.Value != null))).Select(c => (int)c.Position);

            var mtiBytes = message.MTI.BuildFieldValue(message.MTI.Value);

            var messageBytes = new List<byte>(mtiBytes);
            messageBytes.AddRange(BuildBitMap(message.BitMap, orderedFieldPositions));

            IEnumerable<int> fieldsToBuild = orderedFieldPositions.Where(pos => pos != (int)IsoFields.F1 && pos != (int)IsoFields.BitMap && pos != (int)IsoFields.MTI);
            BuildFields(fieldsToBuild, message, ref messageBytes);

            return messageBytes.ToArray();
        }

        /// <summary>
        /// Adds message bytes the rest of the Fields
        /// </summary>
        /// <param name="orderedFieldPositions">fields that need to be builded</param>
        /// <param name="message">message instance</param>
        /// <param name="messageBytes">message bytes as Ref</param>
        private void BuildFields(IEnumerable<int> orderedFieldPositions, IIsoMessage message, ref List<byte> messageBytes)
        {
            foreach (var fieldPosition in orderedFieldPositions)
            {
                IIsoFieldProperties fieldProperties = message.GetFieldByPosition(fieldPosition);

                if (fieldProperties != null)
                {
                    var valueForMessage = fieldProperties.Value;

                    if (fieldProperties is IsoField isoField && isoField.Tags != null)
                    {
                        IEnumerable<byte> customFieldBytes = BuildTagFields(isoField);
                        messageBytes.AddRange(fieldProperties.BuildCustomFieldLentgh(customFieldBytes.Count().ToString()));
                        messageBytes.AddRange(customFieldBytes);
                    }
                    else
                        messageBytes.AddRange(fieldProperties.BuildFieldValue(valueForMessage.ToString()));
                }
            }
        }

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

            for (var bitPosition = 1; bitPosition <= bitMapFields.Count - 1; bitPosition++)
            {
                var bitMapValue = bitMapFields[bitPosition];

                //Then Field at BitPosition exists and Should Be Parsed
                if (bitMapValue == '1')
                {
                    IIsoFieldProperties fieldProperties = isoMessage.GetFieldByPosition(bitPosition + 1);

                    if(fieldProperties != null)
                    {
                        if (fieldProperties is IsoField isoField && isoField.Tags != null)
                        {
                            var customFieldLenBytes = fieldProperties.TagFieldLength(isoMessageBytes, ref currentPos);
                            ParseTagFields(ref isoField, isoMessageBytes.Skip(currentPos).Take(customFieldLenBytes).ToArray());
                            isoMessage.SetFieldValue(bitPosition + 1, string.Empty);
                            currentPos = currentPos + customFieldLenBytes;
                            continue;
                        }

                        var fieldValue = fieldProperties.GetFieldValue(isoMessageBytes, ref currentPos);
                        isoMessage.SetFieldValue(bitPosition + 1, fieldValue);
                    }
                }
            }
        }

        /// <summary>
        /// Build Tag Fields Bytes
        /// </summary>
        /// <param name="isoField">Iso Field with Tags</param>
        /// <returns>custom field bytes value</returns>
        private IEnumerable<byte> BuildTagFields(IsoField isoField)
        {
            var customFieldBytes = new List<byte>();

            foreach (ITagProperties tagProperties in isoField.Tags)
            {
                if (tagProperties.Value == null)
                    continue;

                customFieldBytes.AddRange(tagProperties.GetTagBytes(tagProperties.Value));
            }

            return customFieldBytes;
        }

        /// <summary>
        /// Parses IsoField Tags Values
        /// </summary>
        /// <param name="isoField">iso field object</param>
        /// <param name="customFieldBytes">bytes to parse</param>
        /// <returns>bytes read for tags field</returns>
        private int ParseTagFields(ref IsoField isoField, byte[] customFieldBytes)
        {
            var currentPos = 0;

            foreach (ITagProperties tagProperties in isoField.Tags)
            {
                (var tagName, var tagValue) = tagProperties.GetTagValue(customFieldBytes, ref currentPos);
                isoField.SetTagValue(tagName, tagValue);
            }

            return currentPos;
        }

    }
}
