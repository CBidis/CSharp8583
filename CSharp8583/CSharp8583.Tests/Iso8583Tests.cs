using CSharp8583.Common;
using CSharp8583.Models;
using CSharp8583.Tests.Messages;
using System;
using Xunit;

namespace CSharp8583.Tests
{
    public class Iso8583Tests
    {
        private readonly Iso8583 _iso8583;
        public Iso8583Tests() => _iso8583 = new Iso8583();

        [Fact]
        public void Duplicate_Message_ISO_Fields_Should_Throw_ArgException()
        {
            try
            {
                var duplicateMessage = new DuplicateFieldsMessage();
            }
            catch (ArgumentException)
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }

        [Fact]
        public void ASCII_Message_All_ISO_Fields_Parsed()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytes));
            ASCIIMessage asciiMessage = _iso8583.Parse<ASCIIMessage>(ConstantValues.ASCIIBytes);
            Assert.NotNull(asciiMessage);

            Assert.Equal("1038040008C00000", asciiMessage.BitMap);
            Assert.Equal("0100", asciiMessage.MTI);
            Assert.Equal("000000004444", asciiMessage.Field4);
            Assert.Equal("000021", asciiMessage.Field11);
            Assert.Equal("104212", asciiMessage.Field12);
            Assert.Equal("0529", asciiMessage.Field13);
            Assert.Equal("021", asciiMessage.Field22);
            Assert.Equal("000000001015", asciiMessage.Field37);
            Assert.Equal("JI091003", asciiMessage.Field41);
            Assert.Equal("000000000111111", asciiMessage.Field42);
        }

        [Fact]
        public void ASCII_Message_All_ISO_Fields_Parsed_With_New_Method()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytes));
            IIsoMessage asciiMessage = _iso8583.Parse(ConstantValues.ASCIIBytes, ASCIIMessage.GetMessage());
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
        public void ASCII_Message_ISO_Build()
        {
            ASCIIMessage asciiMessage = GetASCIIMessageObj();
            var asciiMessageBytes = _iso8583.Build(asciiMessage, "0100", IsoFields.F39, IsoFields.F73);
            Console.Write(Iso8583.GetRawDebug(asciiMessageBytes));
            Assert.Equal(asciiMessageBytes, ConstantValues.ASCIIBytes);
        }

        [Fact]
        public void ASCII_Message_Missing_Field42_Should_Throw_ParseException()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesMissingF41));
            ASCIIMessage asciiMessage = _iso8583.Parse<ASCIIMessage>(ConstantValues.ASCIIBytesMissingF41);
        }

        [Fact]
        public void ASCII_Message_Build_Missing_Field42Value_Should_Throw_ArgNullException()
        {
            ASCIIMessage asciiMessage = GetASCIIMessageObj();
            asciiMessage.Field42 = null;

            try
            {
                var asciiMessageBytes = _iso8583.Build(asciiMessage, "0100", IsoFields.F39, IsoFields.F73);
            }
            catch (ArgumentNullException)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void Parse_ASCII_Message_With_Secondary_BitMap()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesWithSecondaryBitmap));
            ASCIIMessage asciiMessage = _iso8583.Parse<ASCIIMessage>(ConstantValues.ASCIIBytesWithSecondaryBitmap);

            Assert.NotNull(asciiMessage);
            Assert.Equal(32, asciiMessage.BitMap.Length);
            Assert.Equal("9038040008C000000080000000000000", asciiMessage.BitMap);
            Assert.Equal("0100", asciiMessage.MTI);
            Assert.Equal("000000004444", asciiMessage.Field4);
            Assert.Equal("000021", asciiMessage.Field11);
            Assert.Equal("104212", asciiMessage.Field12);
            Assert.Equal("0529", asciiMessage.Field13);
            Assert.Equal("021", asciiMessage.Field22);
            Assert.Equal("000000001015", asciiMessage.Field37);
            Assert.Equal("JI091003", asciiMessage.Field41);
            Assert.Equal("000000000111111", asciiMessage.Field42);
            Assert.Equal("121212", asciiMessage.Field73);
        }

        [Fact]
        public void Build_ASCII_Message_Secondary_BitMap()
        {
            ASCIIMessage asciiMessage = GetASCIIMessageObj();
            asciiMessage.Field73 = "121212";

            var asciiMessageBytes = _iso8583.Build(asciiMessage, "0100", IsoFields.F39);
            Console.Write(Iso8583.GetRawDebug(asciiMessageBytes));
            Assert.Equal(asciiMessageBytes, ConstantValues.ASCIIBytesWithSecondaryBitmap);
        }

        [Fact]
        public void Parse_ASCII_Message_With_F63()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesWithResField));
            ASCIIMessageWF63 asciiMessageF63 = _iso8583.Parse<ASCIIMessageWF63>(ConstantValues.ASCIIBytesWithResField);

            Assert.NotNull(asciiMessageF63);
            Assert.NotNull(asciiMessageF63.Field63);
            Assert.Equal("JI091003", asciiMessageF63.Field41);
            Assert.Equal("000000000111111", asciiMessageF63.Field42);
            Assert.Equal("1234", asciiMessageF63.Field63.Tag01);
            Assert.Equal("22222222222", asciiMessageF63.Field63.Tag02);
            Assert.Equal("PROPERTY TAXES", asciiMessageF63.Field63.Tag03);
            Assert.Equal("1111111111111", asciiMessageF63.Field63.Tag04);
        }

        [Fact]
        public void Build_ASCII_Message_With_F63()
        {
            var asciiMessageF63 = new ASCIIMessageWF63
            {
                Field41 = "JI091003",
                Field42 = "000000000111111",
                Field63 = new ResField63
                {
                    Tag01 = "1234",
                    Tag02 = "22222222222",
                    Tag03 = "PROPERTY TAXES",
                    Tag04 = "1111111111111"
                }
            };

            var asciiMessage63Bytes = _iso8583.Build(asciiMessageF63, "0100");
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesWithResField));
            Assert.Equal(asciiMessage63Bytes, ConstantValues.ASCIIBytesWithResField);
        }

        private ASCIIMessage GetASCIIMessageObj() => new ASCIIMessage
        {
            Field4 = "000000004444",
            Field11 = "000021",
            Field12 = "104212",
            Field13 = "0529",
            Field22 = "021",
            Field37 = "000000001015",
            Field41 = "JI091003",
            Field42 = "000000000111111"
        };


        [Fact]
        public void ASCII_Message_Parse_And_Build_TagValueLength()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesTagLengthThree));
            AsciiMessageWTagLenght asciiMessage = _iso8583.Parse<AsciiMessageWTagLenght>(ConstantValues.ASCIIBytesTagLengthThree);
            Assert.Equal("00000005", asciiMessage.Field41);
            Assert.Equal("000000000001112", asciiMessage.Field42);
            Assert.Equal("0000", asciiMessage.Field63.Tag01);
            Assert.Equal("FR4454881", asciiMessage.Field63.Tag02);
            Assert.Equal("22215", asciiMessage.Field63.Tag03);
            Assert.Equal("0", asciiMessage.Field63.Tag04);

            var asciiMessageBytes = _iso8583.Build(asciiMessage, "0100");

            Assert.Equal(asciiMessageBytes, ConstantValues.ASCIIBytesTagLengthThree);
        }


        [Fact]
        public void ASCII_Message_Parse_And_Build_TagValueLengthII()
        {
            Console.Write(Iso8583.GetRawDebug(ConstantValues.ASCIIBytesTagLengthTwo));
            AsciiMessageWTagLengthII asciiMessage = _iso8583.Parse<AsciiMessageWTagLengthII>(ConstantValues.ASCIIBytesTagLengthTwo);
            Assert.Equal("00000005", asciiMessage.Field41);
            Assert.Equal("000000000001112", asciiMessage.Field42);
            Assert.Equal("0000", asciiMessage.Field63.Tag01);
            Assert.Equal("FR4454881", asciiMessage.Field63.Tag02);
            Assert.Equal("22215", asciiMessage.Field63.Tag03);
            Assert.Equal("0", asciiMessage.Field63.Tag04);

            var asciiMessageBytes = _iso8583.Build(asciiMessage, "0100");

            Assert.Equal(asciiMessageBytes, ConstantValues.ASCIIBytesTagLengthTwo);
        }
    }
}