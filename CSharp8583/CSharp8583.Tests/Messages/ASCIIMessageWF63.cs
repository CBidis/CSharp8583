using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Messages;
using CSharp8583.Models;
using System.Collections.Generic;

namespace CSharp8583.Tests.Messages
{
    public class ASCIIMessageWF63 : BaseMessage
    {
        [IsoField(position: IsoFields.F41, maxLen: 8, lengthType: LengthType.FIXED, contentType: ContentType.ANS)]
        public string Field41 { get; set; }

        [IsoField(position: IsoFields.F42, maxLen: 15, lengthType: LengthType.FIXED, contentType: ContentType.ANS)]
        public string Field42 { get; set; }

        [IsoField(position: IsoFields.F63, maxLen: 999, lengthType: LengthType.LLLVAR, contentType: ContentType.ANS)]
        public ResField63 Field63 { get; set; }
    }

    public class ResField63 : CustomField
    {
        [Tag(position: 1, tagName: "01", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag01 { get; set; }

        [Tag(position: 2, tagName: "02", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag02 { get; set; }

        [Tag(position: 3, tagName: "03", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag03 { get; set; }

        [Tag(position: 4, tagName: "04", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag04 { get; set; }
    }
}