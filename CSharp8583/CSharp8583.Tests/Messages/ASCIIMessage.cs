using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Messages;
using CSharp8583.Models;
using System.Collections.Generic;

namespace CSharp8583.Tests.Messages
{
    public class ASCIIMessage : BaseMessage
    {
        [IsoField(position: IsoFields.F4, maxLen: 12, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field4 { get; set; }

        [IsoField(position: IsoFields.F11, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field11 { get; set; }

        [IsoField(position: IsoFields.F12, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field12 { get; set; }

        [IsoField(position: IsoFields.F13, maxLen: 4, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field13 { get; set; }

        [IsoField(position: IsoFields.F22, maxLen: 3, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field22 { get; set; }

        [IsoField(position: IsoFields.F37, maxLen: 12, lengthType: LengthType.FIXED, contentType: ContentType.AN)]
        public string Field37 { get; set; }

        [IsoField(position: IsoFields.F39, maxLen: 2, lengthType: LengthType.FIXED, contentType: ContentType.AN)]
        public ResponseCodes Field39 { get; set; }

        [IsoField(position: IsoFields.F41, maxLen: 8, lengthType: LengthType.FIXED, contentType: ContentType.ANS)]
        public string Field41 { get; set; }

        [IsoField(position: IsoFields.F42, maxLen: 15, lengthType: LengthType.FIXED, contentType: ContentType.ANS)]
        public string Field42 { get; set; }

        [IsoField(position: IsoFields.F73, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.ANS)]
        public string Field73 { get; set; }
    }

    public enum ResponseCodes
    {
        [EnumIsoValue("00")]
        Success,
        [EnumIsoValue("96")]
        Error
    }
}
