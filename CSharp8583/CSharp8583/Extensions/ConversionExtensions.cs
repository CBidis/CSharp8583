using CSharp8583.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharp8583.Extensions
{
    /// <summary>
    /// Contains Common Conversion Extensions
    /// For BCD check --> https://gist.github.com/neuro-sys/953548
    /// </summary>
    public static class ConversionExtensions
    {
        /// <summary>
        /// Converts Byte Array to HEX string representation
        /// </summary>
        /// <param name="hexBytes"></param>
        /// <returns></returns>
        public static string ToHexString(this IEnumerable<byte> hexBytes)
        {
            if (hexBytes == null) return null;
            if (hexBytes.Count() == 0) return string.Empty;

            var s = new StringBuilder();
            foreach (var b in hexBytes)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }

        /// <summary>
        /// Converts Byte array of BCD Bytes to Int Source
        /// </summary>
        /// <param name="bcdBytes">bcd bytes</param>
        /// <returns>int representation</returns>
        public static string BDCToString(this byte[] bcdBytes)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < bcdBytes.Length; i++)
            {
                sb.Append(BCDtoString(bcdBytes[i]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts Byte to BCD String Representation
        /// </summary>
        /// <param name="bcd">bcd byte</param>
        /// <returns>String of BCD Byte</returns>
        private static string BCDtoString(byte bcd)
        {
            var sb = new StringBuilder();

            var high = (byte)(bcd & 0xf0);
            high >>= (byte)4;
            high = (byte)(high & 0x0f);
            var low = (byte)(bcd & 0x0f);

            sb.Append(high);
            sb.Append(low);

            return sb.ToString();
        }

        /// <summary>
        /// Converts Int Value to BCD Bytes, /// Another Example Source : https://gist.github.com/Pellared/7d61fdb2dc9a9799dfc8
        /// </summary>
        /// <param name="intValue">int value</param>
        /// <returns>BCD Bytes</returns>
        public static byte[] IntToBCD(this long intValue)
        {
            var digits = 0;
            var temp = intValue;
            while (temp != 0)
            {
                digits++;
                temp /= 10;
            }

            var byteLen = digits % 2 == 0 ? digits / 2 : (digits + 1) / 2;

            var bcd = new byte[byteLen];
            for (var i = 0; i < digits; i++)
            {
                var tmp = (byte)(intValue % 10);
                if (i % 2 == 0)
                {
                    bcd[i / 2] = tmp;
                }
                else
                {
                    bcd[i / 2] |= (byte)(tmp << 4);
                }
                intValue /= 10;
            }

            for (var i = 0; i < byteLen / 2; i++)
            {
                var tmp = bcd[i];
                bcd[i] = bcd[byteLen - i - 1];
                bcd[byteLen - i - 1] = tmp;
            }

            return bcd;
        }

        /// <summary>
        /// Converts string BCD Value to BCD Bytes
        /// </summary>
        /// <param name="bcdString">bcd string value</param>
        /// <param name="isLittleEndian">isLittleIndian or BigIndian</param>
        /// <param name="bytesLen">number of bytes to create</param>
        /// <returns></returns>
        public static byte[] ConvertToBinaryCodedDecimal(this string bcdString, bool isLittleEndian, int bytesLen = 0)
        {
            var isValid = true;

            isValid = isValid && !string.IsNullOrEmpty(bcdString);

            byte[] bytes;
            if (isValid)
            {
                var chars = bcdString.ToCharArray();
                var len = chars.Length / 2;

                if (bytesLen != 0 && len != bytesLen)
                {                  
                    while(len != bytesLen)
                    {
                        bcdString = "0" + bcdString;
                        chars = bcdString.ToCharArray();
                        len = chars.Length / 2;
                    }
                }

                bytes = new byte[len];
                if (isLittleEndian)
                {
                    for (var i = 0; i < len; i++)
                    {
                        var highNibble = byte.Parse(chars[2 * (len - 1) - 2 * i].ToString());
                        var lowNibble = byte.Parse(chars[2 * (len - 1) - 2 * i + 1].ToString());
                        bytes[i] = (byte)((byte)(highNibble << 4) | lowNibble);
                    }
                }
                else
                {
                    for (var i = 0; i < len; i++)
                    {
                        var highNibble = byte.Parse(chars[2 * i].ToString());
                        var lowNibble = byte.Parse(chars[2 * i + 1].ToString());
                        bytes[i] = (byte)((byte)(highNibble << 4) | lowNibble);
                    }
                }
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "Input string ({0}) was invalid.", bcdString));
            }
            return bytes;
        }

        /// <summary>
        /// Converts Int Value to Hex Value
        /// </summary>
        /// <param name="intValue">Int value</param>
        /// <param name="padLength">pad length with zeroes</param>
        /// <returns>string hex value</returns>
        public static string IntToHexValue(this int intValue, int padLength) => intValue.ToString("X").PadLeft(padLength, '0');

        /// <summary>
        /// Converts Hex Value to Int
        /// </summary>
        /// <param name="hexValue">hex value</param>
        /// <returns>int value</returns>
        public static int HexValueToInt(this string hexValue) => int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

        /// <summary>
        /// Converts Bytes to Binary String Representation
        /// </summary>
        /// <param name="bytes">bytes to convert</param>
        /// <returns>Binary String</returns>
        public static string ToBinaryString(this IEnumerable<byte> bytes) => string.Join(string.Empty,
                                                                                 bytes.Select(x => Convert.ToString(x, 2).PadLeft(4, '0')));

        /// <summary>
        /// Converts Hexadeimal Values to Binary String
        /// </summary>
        /// <param name="hexString">hex values string</param>
        /// <returns>Binary String</returns>
        public static string ToBinaryStringFromHex(this string hexString) => string.Join(string.Empty, hexString.Select(
                                                                                c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

        /// <summary>
        /// Converts Hex string to string Representation
        /// </summary>
        /// <param name="hexString">HEX string Data</param>
        /// <returns>string representation</returns>
        public static string HexStringToString(this string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts bytes to ASCII representation according to given encoding
        /// </summary>
        /// <param name="bytes">bytes to convert</param>
        /// <param name="encoding">encoding of Bytes</param>
        /// <returns>ASCII String</returns>
        public static string ToASCIIString(this IEnumerable<byte> bytes, EncodingType encoding) => encoding == EncodingType.None ? Encoding.ASCII.GetString(bytes.ToArray()) : Encoding.GetEncoding((int)encoding).GetString(bytes.ToArray());

        /// <summary>
        /// Converts ASCII Data to bytes according to given encoding
        /// </summary>
        /// <param name="asciiData">ascii data</param>
        /// <param name="encoding">encoding of Bytes</param>
        /// <returns>ascii data bytes</returns>
        public static byte[] FromASCIIToBytes(this string asciiData, EncodingType encoding) => encoding == EncodingType.None ? Encoding.ASCII.GetBytes(asciiData) : Encoding.GetEncoding((int)encoding).GetBytes(asciiData);

        /// <summary>
        /// Converts Hex Bytes to string Representation
        /// </summary>
        /// <param name="bytes">hex bytes</param>
        /// <returns>string representation</returns>
        public static string HexBytesToString(this IEnumerable<byte> bytes) => BitConverter.ToString(bytes.ToArray()).Replace("-", string.Empty).HexStringToString();


        /// <summary>
        /// Deconstructor Extension method for Key/Value Pairs
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="tuple"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }
    }
}