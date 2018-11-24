using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Messages;

namespace CSharp8583.Tests.Messages
{
    public class DuplicateFieldsMessage : BaseMessage
    {
        [IsoField(position: IsoFields.F12, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field11 { get; set; }

        [IsoField(position: IsoFields.F12, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field12 { get; set; }

        [IsoField(position: IsoFields.F13, maxLen: 4, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field13 { get; set; }
    }
}
