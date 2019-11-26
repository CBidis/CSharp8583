using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Extensions;
using CSharp8583.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static CSharp8583.Common.Utils;

namespace CSharp8583
{
    /// <summary>
    /// Used to Build/Parse Iso Messages
    /// </summary>
    public partial class Iso8583
    {
        /// <summary>
        /// Parse a byte[] iso Message to an Instance of TMessage
        /// </summary>
        /// <typeparam name="TMessage">TMessage class, a derived class of BaseMessage class</typeparam>
        /// <param name="isoMessageBytes">bytes of received Message</param>
        /// <returns>an Instance of TMessage</returns>
        public TMessage Parse<TMessage>(byte[] isoMessageBytes) where TMessage : BaseMessage, new()
        {
            var currentPosition = 0;
            var messageIntance = new TMessage();

            IsoFieldAttribute mtiIsoField = messageIntance.GetIsoFieldByPropName(nameof(messageIntance.MTI));
            messageIntance.MTI = mtiIsoField.GetFieldValue(isoMessageBytes, ref currentPosition);

            IsoFieldAttribute bitMapField = messageIntance.GetIsoFieldByPropName(nameof(messageIntance.BitMap));
            messageIntance.BitMap = ParseBitMap(bitMapField, isoMessageBytes, ref currentPosition);

            ParseFields(ref messageIntance, isoMessageBytes, currentPosition);
            return messageIntance;
        }

        /// <summary>
        /// Parse BitMap of Message to be Parsed
        /// </summary>
        /// <param name="bitMapProperties">bit map field properties</param>
        /// <param name="isoMessageBytes">messageBytes</param>
        /// <param name="currentPos">position to Read</param>
        /// <returns>BitMap Value</returns>
        private string ParseBitMap(IIsoFieldProperties bitMapProperties, byte[] isoMessageBytes, ref int currentPos)
        {
            var bitMap = bitMapProperties.GetFieldValue(isoMessageBytes, ref currentPos);

            //Second BitMap Exists Read More Data
            if (bitMap.ToBinaryStringFromHex().First() == '1')
                return bitMap + bitMapProperties.GetFieldValue(isoMessageBytes, ref currentPos);

            return bitMap;
        }

        /// <summary>
        /// Builds Byte[] from Instance of Message
        /// </summary>
        /// <typeparam name="TMessage">TMessage class, a derived class of BaseMessage class</typeparam>
        /// <param name="message">message instance</param>
        /// <param name="MTI">MTI Response value</param>
        /// <param name="notIncludeFields">whether the BitMap will Contain Fields without values</param>
        /// <returns>byte array Iso Message</returns>
        public byte[] Build<TMessage>(TMessage message, string MTI, params IsoFields[] notIncludeFields) where TMessage : BaseMessage, new()
        {
            IEnumerable<int> orderedFieldPositions = message.MessagePropsIsoFieldAttributes.Select(c => (int)c.Value.Position);

            IsoFieldAttribute mtiIsoField = message.GetIsoFieldByPropName(nameof(message.MTI));
            var mtiBytes = mtiIsoField.BuildFieldValue(MTI);

            var messageBytes = new List<byte>(mtiBytes);

            IEnumerable<int> fieldsForBitMap = orderedFieldPositions.Where(pos => !notIncludeFields.Contains((IsoFields)pos));
            messageBytes.AddRange(BuildBitMap(message, fieldsForBitMap));

            IEnumerable<int> fieldsToBuild = fieldsForBitMap.Where(pos => pos != (int)IsoFields.F1  && pos != (int)IsoFields.BitMap && pos != (int)IsoFields.MTI);
            BuildFields(fieldsToBuild, message, ref messageBytes);
            return messageBytes.ToArray();
        }

        /// <summary>
        /// Builds BitMap value of Message to Send
        /// </summary>
        /// <typeparam name="TMessage">TMessage class, a derived class of BaseMessage class</typeparam>
        /// <param name="message">message instance</param>
        /// <param name="orderedFields">ordered fields positions</param>
        /// <returns>byte array bitMap value</returns>
        private byte[] BuildBitMap<TMessage>(TMessage message, IEnumerable<int> orderedFields) where TMessage : BaseMessage, new()
        {
            var secondBitRequired = orderedFields.Any(pos => pos > 65 && pos < 128);
            IsoFieldAttribute bitMapIsoField = message.GetIsoFieldByPropName(nameof(BaseMessage.BitMap));
            char[] bitmapBinaryArray = null;

            if (secondBitRequired)
            {
                bitmapBinaryArray = new char[129];
                bitmapBinaryArray[1] = '1';
            }
            else
            {
                bitmapBinaryArray = new char[65];
                bitmapBinaryArray[1] = '0';
            }
            //Building BitMap
            for (var i = 2; i < bitmapBinaryArray.Length; i++)
            {
                if (orderedFields.Contains(i))
                    bitmapBinaryArray[i] = '1';
                else
                    bitmapBinaryArray[i] = '0';
            }

            var bitmapString = new string(bitmapBinaryArray);
            var bitMap = Convert.ToInt64(bitmapString.Substring(1, 64), 2).ToString("X").PadLeft(16, '0');

            if (secondBitRequired)
            {
                bitMap = bitMap + Convert.ToInt64(bitmapString.Substring(65, 64), 2).ToString("X").PadLeft(16, '0');
            }

            return bitMapIsoField.BuildFieldValue(bitMap);
        }

        /// <summary>
        /// Parse Fields and Assign Values according to BitMap Value
        /// </summary>
        /// <typeparam name="TMessage">TMessage class, a derived class of BaseMessage class</typeparam>
        /// <param name="message">message to set values of ISO Message</param>
        /// <param name="isoMessageBytes">bytes of received Message</param>
        /// <param name="startPosition">position of byte Array message</param>
        private void ParseFields<TMessage>(ref TMessage message, byte[] isoMessageBytes, int startPosition) where TMessage : BaseMessage, new()
        {
            var currentPos = startPosition;
            //Get BitMap Fields List Values
            var bitMapFields = message.BinaryBitMap.ToList();

            for (var bitPosition = 1; bitPosition <= bitMapFields.Count - 1; bitPosition++)
            {
                var bitMapValue = bitMapFields[bitPosition];

                //Then Field at BitPosition exists and Should Be Parsed
                if (bitMapValue == '1')
                {
                    (PropertyInfo prop, IsoFieldAttribute propAttr) = message.GetPropAndFieldAttrByPos(bitPosition + 1);

                    if (prop != null && propAttr != null)
                    {
                        if (prop.PropertyType.BaseType == typeof(CustomField))
                        {
                            var customFieldLenBytes = propAttr.TagFieldLength(isoMessageBytes, ref currentPos);
                            prop.SetValue(message, ParseTagFields(prop.PropertyType, isoMessageBytes.Skip(currentPos).Take(customFieldLenBytes).ToArray()), null);
                            currentPos = currentPos + customFieldLenBytes;
                        }
                        else if (prop.PropertyType == typeof(byte[]))
                        {
                            var customFieldLenBytes = propAttr.TagFieldLength(isoMessageBytes, ref currentPos);
                            prop.SetValue(message, isoMessageBytes.Skip(currentPos).Take(customFieldLenBytes).ToArray(), null);
                            currentPos = currentPos + customFieldLenBytes;
                        }
                        else
                            SetValueToInstance(message, prop, propAttr.GetFieldValue(isoMessageBytes, ref currentPos));
                    }
                }
            }
        }

        /// <summary>
        /// Get raw printed data of the created/received message bytes.
        /// </summary>
        /// <param name="msg">message bytes</param>
        /// <returns>Formatted printed message</returns>
        public static string GetRawDebug(byte[] msg)
        {
            var linebuf = new StringBuilder();

            for (var ii = 0; ii < msg.Length; ii++)
            {
                if (ii % 16 == 0 && ii != 0)
                {
                    linebuf.Append('|');
                    /*Replace non-printable ( between' ' and '~' ) ascii characters with '.'*/
                    for (var jj = ii - 16; jj < ii; jj++)
                    {
                        if (msg[jj] >= 0x20 && msg[jj] <= 0x80)
                            linebuf.Append(Encoding.Default.GetString(msg, jj, 1));
                        else
                            linebuf.Append('.');
                    }
                    linebuf.Append(Environment.NewLine);
                }
                linebuf.Append(string.Format("{0:X2} ", msg[ii]));
            }

            linebuf.Append(new string(' ', (16 - msg.Length % 16) * 3));
            linebuf.Append('|');
            /*Replace non ascii characters of last rolw with '.'*/
            for (var jj = msg.Length - msg.Length % 16; jj < msg.Length; jj++)
            {
                if (msg[jj] >= 0x20 && msg[jj] <= 0x80)//between ' ' and '~' ascii characters
                    linebuf.Append(Encoding.Default.GetString(msg, jj, 1));
                else
                    linebuf.Append('.');
            }

            return linebuf.ToString();
        }

        private CustomField ParseTagFields(Type propertyType, byte[] customFieldBytes)
        {
            var currentPos = 0;
            var instance = (CustomField)Activator.CreateInstance(propertyType);

            foreach((PropertyInfo propInfo, TagAttribute propTagAttr)  in instance.TagPropsIsoFieldAttributes)
            {
                (var tagName, var tagValue) = propTagAttr.GetTagValue(customFieldBytes, ref currentPos);
                PropertyInfo customFieldProp = instance.GetPropByName(tagName);

                if (customFieldProp == null)
                    continue;

                SetValueToInstance(instance, customFieldProp, tagValue);
            }

            return instance;
        }


        private IEnumerable<byte> BuildTagFields(CustomField valueForMessage)
        {
            var customFieldBytes = new List<byte>();

            foreach ((PropertyInfo propInfo, TagAttribute propTagAttr) in valueForMessage.TagPropsIsoFieldAttributes)
            {
                var tagFieldValue = GetValueFromInstance(valueForMessage, propInfo);

                if (tagFieldValue == null)
                    continue;

                customFieldBytes.AddRange(propTagAttr.GetTagBytes(tagFieldValue.ToString()));
            }

            return customFieldBytes;
        }

        /// <summary>
        /// Adds message bytes the rest of the Fields
        /// </summary>
        /// <typeparam name="TMessage">TMessage class, a derived class of BaseMessage class</typeparam>
        /// <param name="orderedFieldPositions">fields that need to be builded</param>
        /// <param name="message">message instance</param>
        /// <param name="messageBytes">message bytes as Ref</param>
        private void BuildFields<TMessage>(IEnumerable<int> orderedFieldPositions, TMessage message, ref List<byte> messageBytes) where TMessage : BaseMessage, new()
        {
            foreach (var fieldPosition in orderedFieldPositions)
            {
                (PropertyInfo prop, IsoFieldAttribute propAttr) = message.GetPropAndFieldAttrByPos(fieldPosition);

                if (prop != null && propAttr != null)
                {
                    var valueForMessage = GetValueFromInstance(message, prop);

                    if (valueForMessage == null)
                        throw new ArgumentNullException($"Iso Field {propAttr?.Position} has null Value");

                    if (prop.PropertyType == typeof(byte[]))
                    {
                        messageBytes.AddRange((byte[])valueForMessage);
                    }
                    else if (prop.PropertyType.BaseType == typeof(CustomField))
                    {
                        IEnumerable<byte> customFieldBytes = BuildTagFields((CustomField)valueForMessage);
                        //Adding Length of Custom Field
                        messageBytes.AddRange(propAttr.BuildCustomFieldLentgh(customFieldBytes.Count().ToString()));
                        messageBytes.AddRange(customFieldBytes);
                    }
                    else
                        messageBytes.AddRange(propAttr.BuildFieldValue(valueForMessage.ToString()));
                }
            }
        }
    }
}
