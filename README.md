# CSharp8583
C# Implementation of the ISO-8583 banking protocol

The CSharp8583 Library is an implementation of the [ISO-8583](https://en.wikipedia.org/wiki/ISO_8583) Protocol as a .NET Standard 2.0 Library.

The library provides the following functions,

```csharp
TMessage Parse<TMessage>(byte[] isoMessageBytes) where TMessage : BaseMessage
```

```csharp
byte[] Build<TMessage>(TMessage message, string MTI, params IsoFields[] notIncludeFields) where TMessage : BaseMessage
```

#### HOW_TO_USE ####

In order to use the library for build/parse of Iso Messages you need to create your Message Classes. Message classes should derive from 
```csharp
BaseMessage class
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

ASCIIMessage = new ASCIIMessage
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

var messageBytes = _iso8583.Build(asciiMessage, "0100", IsoFields.F39);

```

* Parsing Message Bytes

```csharp

Iso8583 _iso8583 = new Iso8583();
ASCIIMessage asciiMessage = _iso8583.Parse<ASCIIMessage>(messageByteArray);

```

For more test cases and how to use the ibrary please see the unit test project provided.
