using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Messages;

namespace CSharp8583.Tests.Messages
{
    public class AsciiMessageWTagLenght : BaseMessage
    {

        [IsoField(position: IsoFields.F41, maxLen: 8, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field41 { get; set; }

        [IsoField(position: IsoFields.F42, maxLen: 15, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field42 { get; set; }

        [IsoField(position: IsoFields.F63, maxLen: 999, lengthType: LengthType.LLLVAR, contentType: ContentType.ANS)]
        public Field63 Field63 { get; set; }
    }

    public class Field63 : CustomField
    {
        [Tag(position: 1, tagName: "01", lenBytesLen: 3, lenDataType: DataType.ASCII, dataType: DataType.ASCII)]
        public virtual string Tag01 { get; set; }

        [Tag(position: 2, tagName: "02", lenBytesLen: 3, lenDataType: DataType.ASCII, dataType: DataType.ASCII)]
        public virtual string Tag02 { get; set; }

        [Tag(position: 3, tagName: "03", lenBytesLen: 3, lenDataType: DataType.ASCII, dataType: DataType.ASCII)]
        public virtual string Tag03 { get; set; }

        [Tag(position: 4, tagName: "04", lenBytesLen: 3, lenDataType: DataType.ASCII, dataType: DataType.ASCII)]
        public virtual string Tag04 { get; set; }
    }
}