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
    /// Extension Methods for Tag Fields Parsing
    /// </summary>
    internal static class TagFieldExtensions
    {
        /// <summary>
        /// Retrieves all Tag Fields of TField Instance
        /// </summary>
        /// <typeparam name="TField">TField type as derived class from CustomField</typeparam>
        /// <param name="fieldType">type of Field</param>
        /// <returns>A Dictionary of TField Properties and TField Attributes of TField Class</returns>
        internal static Dictionary<PropertyInfo, TagAttribute> PropsAndTagsdAttributes<TField>(this Type fieldType) where TField : CustomField
        {
#if NET40
           IEnumerable<PropertyInfo> tagFieldProps = fieldType?.GetProperties().Where(
                                        prop => Attribute.GetCustomAttributes(prop).Any(attr => attr.GetType() == typeof(TagAttribute)));
#else
            IEnumerable<PropertyInfo> tagFieldProps = fieldType?.GetProperties().Where(
                prop => prop.GetCustomAttributes().Any(attr => attr.GetType() == typeof(TagAttribute)));
#endif


            if (!tagFieldProps.Any())
                throw new ArgumentException($"for type {fieldType?.FullName} they have not been defined any properties with attibute of TagAttribute");

            var tagPropsAttributesOfT = new Dictionary<PropertyInfo, TagAttribute>();

            foreach (PropertyInfo tagFieldProp in tagFieldProps)
            {
#if NET40
                TagAttribute tagFieldFieldAttr = (TagAttribute)Attribute.GetCustomAttribute(tagFieldProp, typeof(TagAttribute));
#else
                TagAttribute tagFieldFieldAttr = tagFieldProp.GetCustomAttribute<TagAttribute>();
#endif

                tagPropsAttributesOfT.Add(tagFieldProp, tagFieldFieldAttr);
            }

            return tagPropsAttributesOfT;
        }

        /// <summary>
        /// Get Tag Value of Custom Field
        /// </summary>
        /// <param name="tagProperties">Tag Properties object</param>
        /// <param name="fieldBytes">bytes of Custom Field</param>
        /// <param name="currentPos">current position</param>
        /// <returns>Tag Name and Tag Value Tuple</returns>
        internal static (string, string) GetTagValue(this ITagProperties tagProperties, byte[] fieldBytes, ref int currentPos)
        {
            var tagName = string.Empty;
            var fieldValue = string.Empty;
            var lengthBytes = tagProperties.LenBytesLen;
            var tagBytes = tagProperties.TagBytesLen;
            try
            {
                if (tagProperties.IsTLV)
                {
                    tagName = fieldBytes.Skip(currentPos).Take(tagBytes).ToASCIIString(tagProperties.Encoding);
                    currentPos = currentPos + tagBytes;
                }

                int fieldLen;
                switch (tagProperties.LenDataType)
                {
                    case DataType.ASCII:
                        var lenValue = fieldBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(tagProperties.Encoding);
                        fieldLen = string.IsNullOrEmpty(lenValue) ? 0 : int.Parse(lenValue);
                        break;
                    case DataType.HEX:
                        var lenValueH = fieldBytes.Skip(currentPos).Take(lengthBytes).ToASCIIString(tagProperties.Encoding);
                        fieldLen = string.IsNullOrEmpty(lenValueH) ? 0 : lenValueH.HexValueToInt();
                        break;
                    case DataType.BCD:
                        var lenValueBCD = fieldBytes.Skip(currentPos).Take(lengthBytes).ToArray().BDCToString();
                        fieldLen = string.IsNullOrEmpty(lenValueBCD) ? 0 : int.Parse(lenValueBCD);
                        break;
                    default:
                        throw new ParseTagException(tagProperties, $"Cannot Parse Length value for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}");
                }

                if (!tagProperties.IsTLV)
                {
                    currentPos = currentPos + lengthBytes;
                    tagName = fieldBytes.Skip(currentPos).Take(tagBytes).ToASCIIString(tagProperties.Encoding);
                    fieldLen = fieldLen - tagBytes;
                    currentPos = currentPos + tagBytes;
                }
                else
                    currentPos = currentPos + lengthBytes;

                switch (tagProperties.DataType)
                {
                    case DataType.BIN:
                        fieldValue = fieldBytes.Skip(currentPos).Take(fieldLen).ToStringFromBinary();
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.ASCII:
                        fieldValue = fieldBytes.Skip(currentPos).Take(fieldLen).ToASCIIString(tagProperties.Encoding);
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.HEX:
                        fieldValue = fieldBytes.Skip(currentPos).Take(fieldLen).HexBytesToString();
                        currentPos = currentPos + fieldLen;
                        break;
                    case DataType.BCD:
                        fieldValue = fieldBytes.Skip(currentPos).Take(fieldLen).ToArray().BDCToString();
                        currentPos = currentPos + fieldLen;
                        break;
                    default:
                        throw new ParseTagException(tagProperties, $"Cannot Parse Field value for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}");
                }
            }
            catch (Exception ex)
            {
                throw new ParseTagException(tagProperties, $"Cannot Parse Field value for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}", ex);
            }

            return (tagName, fieldValue);
        }

        internal static byte[] GetTagBytes(this ITagProperties tagProperties, string tagValue)
        {
            var tagFieldBytes = new List<byte>();
            var tagLentgh = tagValue.Length;

            if (tagProperties.DataType == DataType.BCD)
                tagLentgh = tagLentgh % 2 == 0 ? tagLentgh / 2 : (tagLentgh / 2) + 1;

            try
            {
                if (tagProperties.IsTLV)
                {
                    tagFieldBytes.AddRange(tagProperties.TagName.FromASCIIToBytes(tagProperties.Encoding));
                }
                else
                {
                    tagLentgh = tagLentgh + tagProperties.TagName.Length;
                }

                switch (tagProperties.LenDataType)
                {
                    case DataType.ASCII:
                        tagFieldBytes.AddRange(tagLentgh.ToString().PadLeft(tagProperties.LenBytesLen, '0').FromASCIIToBytes(tagProperties.Encoding));
                        break;
                    case DataType.HEX:
                        var intLen = int.Parse(tagLentgh.ToString().PadLeft(tagProperties.LenBytesLen, '0'));
                        tagFieldBytes.AddRange(intLen.IntToHexValue(tagProperties.LenBytesLen).FromASCIIToBytes(tagProperties.Encoding));
                        break;
                    case DataType.BCD:
                        var valueLenBCD = tagLentgh.ToString().PadLeft(tagProperties.LenBytesLen, '0').ConvertToBinaryCodedDecimal(false, tagProperties.LenBytesLen);
                        tagFieldBytes.AddRange(valueLenBCD);
                        break;
                    default:
                        throw new BuildTagException(tagProperties, $"Cannot Build Length value for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}");
                }

                if (!tagProperties.IsTLV)
                {
                    tagFieldBytes.AddRange(tagProperties.TagName.FromASCIIToBytes(tagProperties.Encoding));
                }

                switch (tagProperties.DataType)
                {
                    case DataType.BIN:
                        tagFieldBytes.AddRange(tagValue.ToBinaryStringFromHex().ToBytesFromBinaryString());
                        break;
                    case DataType.BCD:
                        var bcdValue = tagValue.ConvertToBinaryCodedDecimal(false);
                        tagFieldBytes.AddRange(bcdValue);
                        break;
                    case DataType.ASCII:
                    case DataType.HEX:
                        tagFieldBytes.AddRange(tagValue.FromASCIIToBytes(tagProperties.Encoding));
                        break;
                    default:
                        throw new BuildTagException(tagProperties, $"Cannot Build value {tagValue} for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}");
                }
            }
            catch (Exception ex)
            {
                throw new BuildTagException(tagProperties, $"Cannot Build value {tagValue} for {tagProperties?.TagName} and Len Type {tagProperties?.LenDataType}", ex);
            }

            return tagFieldBytes.ToArray();
        }

    }
}
