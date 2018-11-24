# CSharp8583

The CSharp8583 Library is a C# implementation of the [ISO-8583](https://en.wikipedia.org/wiki/ISO_8583) Protocol as a .NET Standard 2.0 Library.

The library provides the following functions,

```csharp
TMessage Parse<TMessage>(byte[] isoMessageBytes) where TMessage : BaseMessage
```

```csharp
byte[] Build<TMessage>(TMessage message, string MTI, params IsoFields[] notIncludeFields) where TMessage : BaseMessage
```

#### HOW_TO_USE with simple message ####

In order to use the library for build/parse of Iso Messages you need to create your Message Classes. Message classes should derive from 
```csharp
    public class BaseMessage
    {
        /// <summary>
        /// Common Constructor
        /// </summary>
        public BaseMessage()
        {
        /// <summary>
        /// All Messages contain an MTI
        /// </summary>
        [IsoField(position: IsoFields.MTI, maxLen: 4, lengthType: LengthType.FIXED, contentType: ContentType.B, dataType: DataType.ASCII)]
        public virtual string MTI { get; set; }

        /// <summary>
        /// All Messages contain a BitMap, Binary Representation
        /// </summary>
        [IsoField(position: IsoFields.BitMap, maxLen: 8, lengthType: LengthType.FIXED, contentType: ContentType.B, dataType: DataType.HEX)]
        public virtual string BitMap { get; set; }
     }
```

and use Annotations in order to mark the properties of the ISO Message, currently supported types for Properties are byte[], string, CustomField and Enums.

for example,

```csharp

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

```

* Building Message Bytes

```csharp

Iso8583 _iso8583 = new Iso8583();

ASCIIMessage asciiMessage = new ASCIIMessage
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

var messageBytes = _iso8583.Build<ASCIIMessage>(asciiMessage, "0100", IsoFields.F39);

```

* Parsing Message Bytes

```csharp

Iso8583 _iso8583 = new Iso8583();
ASCIIMessage asciiMessage = _iso8583.Parse<ASCIIMessage>(messageBytes);

```

#### HOW_TO_USE With messages containing reserved fields ####

A common case with ISO Messages is to make use of reserved fields (F63) in order to provide additional to data. The reserved fields usually follow the Patterns of TLV(Tag - Length - Value) or LTV (Length - Tag - Value).

In order to use the Library with reserved Fields in the Message class you need to create a property that derives from the CustomField class.

```csharp

    public class IsoMessage : BaseMessage
    {
        [IsoField(position: IsoFields.F4, maxLen: 12, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field4 { get; set; }

        [IsoField(position: IsoFields.F11, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field11 { get; set; }

        [IsoField(position: IsoFields.F12, maxLen: 6, lengthType: LengthType.FIXED, contentType: ContentType.N)]
        public string Field12 { get; set; }

        [IsoField(position: IsoFields.F63, maxLen: 999, lengthType: LengthType.LLLVAR, contentType: ContentType.ANS)]
        public Field63Res Field63 { get; set; }
    }
    
        public class Field63Res : CustomField
    {
        [Tag(position: 0, tagName: "00", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag00 { get; set; }

        [Tag(position: 1, tagName: "01", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag01 { get; set; }

        [Tag(position: 2, tagName: "02", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag02 { get; set; }

        [Tag(position: 3, tagName: "03", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag03 { get; set; }

        [Tag(position: 4, tagName: "04", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag04 { get; set; }

        [Tag(position: 5, tagName: "05", lenBytesLen: 2, lenDataType: DataType.HEX, dataType: DataType.ASCII)]
        public virtual string Tag05 { get; set; }
    }

```

_For more test cases and how to use the ibrary please see the unit test project provided._
