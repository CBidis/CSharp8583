using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Exceptions;
using CSharp8583.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharp8583.Extensions
{
    /// <summary>
    /// Contains Functions required for Parsing/Building ISO Messages
    /// </summary>
    internal static class IsoExtensions
    {
        /// <summary>
        /// Retrieves all IsoField Attributes and Properties of TMessage Class
        /// </summary>
        /// <typeparam name="TMessage">TMessage</typeparam>
        /// <param name="messageType">message type</param>
        /// <returns>A Dictionary of TMessage Properties and IsoField Attributes of TMessage Class</returns>
        internal static Dictionary<PropertyInfo, IsoFieldAttribute> PropsAndIsoFieldAttributes<TMessage>(this Type messageType) where TMessage : BaseMessage
        {

#if NET40
            IEnumerable<PropertyInfo> isoFieldProps = messageType?.GetProperties().Where(
                                        prop => Attribute.GetCustomAttributes(prop).Any(attr => attr.GetType() == typeof(IsoFieldAttribute)));
#else
            IEnumerable<PropertyInfo> isoFieldProps = messageType?.GetProperties().Where(
                                        prop => prop.GetCustomAttributes().Any(attr => attr.GetType() == typeof(IsoFieldAttribute)));
#endif

            if (!isoFieldProps.Any())
                throw new ArgumentException($"for type {messageType?.FullName} they have not been defined any properties with attibute of IsoFieldAttribute");

            var isoAPropsttributesOfT = new Dictionary<PropertyInfo, IsoFieldAttribute>();

            foreach (PropertyInfo isoFieldProp in isoFieldProps)
            {
#if NET40
                IsoFieldAttribute isoFieldFieldAttr = (IsoFieldAttribute)Attribute.GetCustomAttribute(isoFieldProp, typeof(IsoFieldAttribute));
#else
                IsoFieldAttribute isoFieldFieldAttr = isoFieldProp.GetCustomAttribute<IsoFieldAttribute>();
#endif
                isoAPropsttributesOfT.Add(isoFieldProp, isoFieldFieldAttr);
            }

            return isoAPropsttributesOfT;
        }

        /// <summary>
        /// Gets Custom Field Len according to Len Bytes
        /// </summary>
        /// <param name="isoFieldAttribute">iso Field Attribute</param>
        /// <param name="isoMessageBytes">iso message bytes</param>
        /// <param name="currentPos">current parsing position</param>
        /// <returns>length of Custom Field parsed</returns>
        internal static int TagFieldLength(this IsoFieldAttribute isoFieldAttribute, byte[] isoMessageBytes, ref int currentPos)
        {
            var fieldLen = 0;
            var fieldValue = string.Empty;
            var lengthBytes = (int)isoFieldAttribute.LengthType;

            if (isoFieldAttribute.LenDataType == DataType.ASCII)
            {
                var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(isoFieldAttribute.Encoding);
                fieldLen = int.Parse(lenValue);
            }
            else if (isoFieldAttribute.LenDataType == DataType.HEX)
            {
                var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(isoFieldAttribute.Encoding);
                fieldLen = lenValue.HexValueToInt();
            }
            else if (isoFieldAttribute.LenDataType == DataType.BCD)
            {
                lengthBytes = lengthBytes - 1;
                var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToArray().BDCToString();
                fieldLen = int.Parse(lenValue);
            }

            currentPos = currentPos + lengthBytes;

            return fieldLen;
        }

        internal static byte[] BuildCustomFieldLentgh(this IsoFieldAttribute isoFieldAttribute, string lenValue)
        {
            lenValue = lenValue.PadLeft((int)isoFieldAttribute.LengthType, '0');

            switch (isoFieldAttribute.LenDataType)
            {
                case DataType.ASCII:
                    return lenValue.FromASCIIToBytes(isoFieldAttribute.Encoding);
                case DataType.HEX:
                    var intLen = int.Parse(lenValue);
                    return intLen.IntToHexValue((int)isoFieldAttribute.LengthType).FromASCIIToBytes(isoFieldAttribute.Encoding);
                case DataType.BCD:
                    return lenValue.ConvertToBinaryCodedDecimal(false, (int)isoFieldAttribute.LengthType - 1);
                default:
                    throw new BuildFieldException(isoFieldAttribute, $"Cannot Parse Length value for {isoFieldAttribute?.Position} and Len Type {isoFieldAttribute?.LenDataType}");
            }
        }

        /// <summary>
        /// Parses bytes value to an object value
        /// </summary>
        /// <param name="isoFieldAttribute">iso Field Attribute</param>
        /// <param name="isoMessageBytes">whole Message bytes</param>
        /// <param name="currentPos">current Position of Parsed Message bytes, passed as a Reference</param>
        /// <returns>field object value</returns>
        internal static string GetFieldValue(this IsoFieldAttribute isoFieldAttribute, byte[] isoMessageBytes, ref int currentPos)
        {
            var fieldLen = 0;
            var fieldValue = string.Empty;
            var lengthBytes = (int)isoFieldAttribute.LengthType;

            try
            {
                switch (isoFieldAttribute.LengthType)
                {
                    case LengthType.FIXED:
                    case LengthType.LVAR:
                        fieldLen = isoFieldAttribute.MaxLen;
                        break;

                    case LengthType.LLVAR:
                    case LengthType.LLLVAR:
                        if (isoFieldAttribute.LenDataType == DataType.ASCII)
                        {
                            var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(isoFieldAttribute.Encoding);
                            fieldLen = int.Parse(lenValue);
                        }
                        else if (isoFieldAttribute.LenDataType == DataType.HEX)
                        {
                            var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(isoFieldAttribute.Encoding);
                            fieldLen = lenValue.HexValueToInt();
                        }
                        else if (isoFieldAttribute.LenDataType == DataType.BCD)
                        {
                            lengthBytes = lengthBytes - 1; //BCD Always one byte less for Lentgh
                            var lenValue = isoMessageBytes.Skip(currentPos).Take(lengthBytes).ToArray().BDCToString();
                            fieldLen = int.Parse(lenValue);
                        }
                        break;

                    default:
                        throw new ParseFieldException(isoFieldAttribute, $"Cannot Parse Length value for {isoFieldAttribute?.Position} and Len Type {isoFieldAttribute?.LenDataType}");
                }

                currentPos = currentPos + lengthBytes;

                switch (isoFieldAttribute.DataType)
                {
                    case DataType.BIN:
                        fieldValue = isoMessageBytes.Skip(currentPos).Take(fieldLen).ToStringFromBinary();
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.BCD:
                        if (isoFieldAttribute.ContentType != ContentType.B)
                            fieldLen = fieldLen % 2 == 1 ? (fieldLen / 2 + 1) : fieldLen / 2;

                        fieldValue = isoMessageBytes.Skip(currentPos).Take(fieldLen).ToArray().BDCToString();
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.ASCII:
                        fieldValue = isoMessageBytes.Skip(currentPos).Take(fieldLen).ToASCIIString(isoFieldAttribute.Encoding);
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.HEX:
                        fieldValue = isoMessageBytes.Skip(currentPos).Take(fieldLen * 2).HexBytesToString();
                        currentPos = currentPos + (fieldLen * 2);
                        break;
                    default:
                        throw new ParseFieldException(isoFieldAttribute, $"Cannot Parse value for {isoFieldAttribute?.Position} and Type {isoFieldAttribute?.DataType}");
                }
            }
            catch (Exception ex)
            {
                throw new ParseFieldException(isoFieldAttribute, $"Cannot Parse value for {isoFieldAttribute?.Position} and Type {isoFieldAttribute?.DataType}", ex);
            }

            return fieldValue;
        }

        /// <summary>
        /// Build Bytes from Field Value
        /// </summary>
        /// <param name="isoFieldAttribute">isofield attribute of Property</param>
        /// <param name="fieldValue">field value</param>
        /// <returns>byte array value of Field</returns>
        internal static byte[] BuildFieldValue(this IsoFieldAttribute isoFieldAttribute, string fieldValue)
        {
            var fieldBytes = new List<byte>();

            try
            {
                switch (isoFieldAttribute.LengthType)
                {
                    case LengthType.FIXED:
                        if (fieldValue.Length < isoFieldAttribute.MaxLen)
                            fieldValue = fieldValue?.PadRight(isoFieldAttribute.MaxLen);
                        break;

                    case LengthType.LVAR:
                    case LengthType.LLVAR:
                    case LengthType.LLLVAR:
                        if (isoFieldAttribute.LenDataType == DataType.ASCII)
                        {
                            var valueLen = fieldValue?.Length.ToString().PadLeft((int)isoFieldAttribute.LengthType, '0');
                            fieldBytes.AddRange(valueLen.FromASCIIToBytes(isoFieldAttribute.Encoding));
                        }
                        else if (isoFieldAttribute.LenDataType == DataType.HEX)
                        {
                            var valueLen = fieldValue?.Length.IntToHexValue((int)isoFieldAttribute.LengthType);
                            fieldBytes.AddRange(valueLen.FromASCIIToBytes(isoFieldAttribute.Encoding));
                        }
                        else if (isoFieldAttribute.LenDataType == DataType.BCD)
                        {
                            var valueLen = fieldValue?.Length.ToString().ConvertToBinaryCodedDecimal(false);
                            fieldBytes.AddRange(valueLen);
                        }
                        break;

                    default:
                        throw new BuildFieldException(isoFieldAttribute, $"Cannot Parse Length value for {isoFieldAttribute?.Position} and Len Type {isoFieldAttribute?.LenDataType}");
                }

                switch (isoFieldAttribute.DataType)
                {
                    case DataType.BIN:
                        fieldBytes.AddRange(fieldValue.ToBinaryStringFromHex().ToBytesFromBinaryString());
                        break;
                    case DataType.BCD:
                        if (fieldValue.Length % 2 == 1)
                            fieldValue += '0';
                        var bcdValue = fieldValue.ConvertToBinaryCodedDecimal(false);
                        fieldBytes.AddRange(bcdValue);
                        break;
                    case DataType.ASCII:
                    case DataType.HEX:
                        fieldBytes.AddRange(fieldValue.FromASCIIToBytes(isoFieldAttribute.Encoding));
                        break;
                    default:
                        throw new BuildFieldException(isoFieldAttribute, $"Cannot Parse value for {isoFieldAttribute?.Position} and Type {isoFieldAttribute?.DataType}");
                }
            }
            catch (Exception ex)
            {
                throw new BuildFieldException(isoFieldAttribute, $"Cannot Parse value for {isoFieldAttribute?.Position} and Type {isoFieldAttribute?.DataType}", ex);
            }

            return fieldBytes.ToArray();
        }
    }
}
