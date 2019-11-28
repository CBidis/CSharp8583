using CSharp8583.Common;
using System;
using Xunit;

namespace CSharp8583.Tests
{
    public class Iso8583PartialTests
    {
        private readonly Iso8583 _iso8583;
        public Iso8583PartialTests() => _iso8583 = new Iso8583();

        [Fact]
        public void ASCII_Message_All_ISO_Fields_Parsed()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytes));
            IIsoMessage asciiMessage = _iso8583.Parse(ConstantValues.ASCIIBytes, ConstantValues.GetDefaultIsoSpecsFromFile());
            Assert.NotNull(asciiMessage);

            Assert.Equal("1038040008C00000", asciiMessage.BitMap.Value);
            Assert.Equal("0100", asciiMessage.MTI.Value);
            Assert.Equal("000000004444", asciiMessage.GetFieldValue(4));
            Assert.Equal("000021", asciiMessage.GetFieldValue(11));
            Assert.Equal("104212", asciiMessage.GetFieldValue(12));
            Assert.Equal("0529", asciiMessage.GetFieldValue(13));
            Assert.Equal("021", asciiMessage.GetFieldValue(22));
            Assert.Equal("000000001015", asciiMessage.GetFieldValue(37));
            Assert.Equal("JI091003", asciiMessage.GetFieldValue(41));
            Assert.Equal("000000000111111", asciiMessage.GetFieldValue(42));
        }

        [Fact]
        public void Parse_ASCII_Message_With_F63()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesWithResField));
            IIsoMessage asciiMessageF63 = _iso8583.Parse(ConstantValues.ASCIIBytesWithResField, ConstantValues.GetDefaultIsoSpecsFromFile());

            Assert.NotNull(asciiMessageF63);
            Assert.NotNull(asciiMessageF63.GetFieldValue(63));
            Assert.Equal("JI091003", asciiMessageF63.GetFieldValue(41));
            Assert.Equal("000000000111111", asciiMessageF63.GetFieldValue(42));
            Assert.Equal("1234", asciiMessageF63.GetTagValue(63, "01"));
            Assert.Equal("22222222222", asciiMessageF63.GetTagValue(63, "02"));
            Assert.Equal("PROPERTY TAXES", asciiMessageF63.GetTagValue(63, "03"));
            Assert.Equal("1111111111111", asciiMessageF63.GetTagValue(63, "04"));
        }

        [Fact]
        public void Parse_ASCII_Message_With_Secondary_BitMap()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesWithSecondaryBitmap));
            IIsoMessage asciiMessage = _iso8583.Parse(ConstantValues.ASCIIBytesWithSecondaryBitmap, ConstantValues.GetDefaultIsoSpecsFromFile());

            Assert.NotNull(asciiMessage);
            Assert.Equal(32, asciiMessage.BitMap.Value.Length);
            Assert.Equal("9038040008C000000080000000000000", asciiMessage.BitMap.Value);
            Assert.Equal("0100", asciiMessage.MTI.Value);
            Assert.Equal("000000004444", asciiMessage.GetFieldValue(4));
            Assert.Equal("000021", asciiMessage.GetFieldValue(11));
            Assert.Equal("104212", asciiMessage.GetFieldValue(12));
            Assert.Equal("0529", asciiMessage.GetFieldValue(13));
            Assert.Equal("021", asciiMessage.GetFieldValue(22));
            Assert.Equal("000000001015", asciiMessage.GetFieldValue(37));
            Assert.Equal("JI091003", asciiMessage.GetFieldValue(41));
            Assert.Equal("000000000111111", asciiMessage.GetFieldValue(42));
            Assert.Equal("121212", asciiMessage.GetFieldValue(73));
        }
    }
}
