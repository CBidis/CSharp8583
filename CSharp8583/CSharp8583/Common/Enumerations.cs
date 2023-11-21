namespace CSharp8583.Common
{
    /// <summary>
    /// Binary Data Format Representation
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// BCD Formatted Values
        /// </summary>
        BCD   = 1,
        /// <summary>
        /// ASCII Values
        /// </summary>
        ASCII = 2,
        /// <summary>
        /// Binary Formatted Values, currently supports Conversion for Base 16 Numbers
        /// </summary>
        BIN   = 3,
        /// <summary>
        /// Hexadecimal Values
        /// </summary>
        HEX   = 4
    }

    /// <summary>
    /// Encoding Types for Parse/Building of byte data  
    /// </summary>
    public enum EncodingType
    {
        /// <summary>
        /// Common ASCII
        /// </summary>
        None = 0,
        /// <summary>
        /// Western European (ISO)
        /// </summary>
        Western_European = 28591,
        /// <summary>
        /// Central European (ISO)
        /// </summary>
        Central_European = 28592,
        /// <summary>
        /// Latin 3 (ISO)
        /// </summary>
        Latin = 28593,
        /// <summary>
        /// Baltic (ISO)
        /// </summary>
        Baltic = 28594,
        /// <summary>
        /// Cyrillic (ISO)  
        /// </summary>
        Cyrillic = 28595,
        /// <summary>
        /// Arabic (ISO)
        /// </summary>
        Arabic = 28596,
        /// <summary>
        /// Greek (ISO)
        /// </summary>
        Greek = 28597,
    }

    /// <summary>
    /// Data Types Values
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// Alpha, including blanks
        /// </summary>
        A = 1,
        /// <summary>
        /// Numeric values only
        /// </summary>
        N = 2,
        /// <summary>
        /// Special characters only
        /// </summary>
        S = 3,
        /// <summary>
        /// Alphanumeric
        /// </summary>
        AN = 4,
        /// <summary>
        /// Alpha and special characters only
        /// </summary>
        AS = 5,
        /// <summary>
        /// Alphabetic, numeric and special characters
        /// </summary>
        ANS = 6,
        /// <summary>
        /// Binary data
        /// </summary>
        B = 7,
        /// <summary>
        /// Tracks 2 and 3 code set as defined in ISO/IEC 7813 and ISO/IEC 4909 respectively
        /// </summary>
        Z = 6
    }

    /// <summary>
    /// Length type Available Values
    /// </summary>
    public enum LengthType
    {
        /// <summary>
        /// Fixed length field of six digits
        /// </summary>
        FIXED = 0,
        /// <summary>
        /// LVAR numeric field of up to 6 digits in length
        /// </summary>
        LVAR = 1,
        /// <summary>
        /// LLVAR alpha field of up to 11 characters in length
        /// </summary>
        LLVAR = 2,
        /// <summary>
        /// LLLVAR binary field of up to 999 bits in length
        /// </summary>
        LLLVAR = 3,
        /// <summary>
        /// LLLLVAR binary field of up to 9999 bits in length
        /// </summary>
        LLLLVAR = 4,
        /// <summary>
        /// LLLLLLVAR binary field of up to 999999 bits in length
        /// </summary>
        LLLLLLVAR = 6
    }

    /// <summary>
    /// All Defined ISO Fields, as F(Number)
    /// </summary>
    public enum IsoFields
    {
        /// <summary>
        /// Starting Field of ISO Message
        /// </summary>
        MTI = 129,

        /// <summary>
        /// Bitmap
        /// </summary>
        BitMap = 0,
        /// <summary>
        ///  Secondary Bitmap
        /// </summary>
        F1 = 1,
        /// <summary>
        ///  Primary account number (PAN)
        /// </summary>
        F2 = 2,
        /// <summary>
        ///  Processing code
        /// </summary>
        F3 = 3,
        /// <summary>
        ///  Amount, transaction
        /// </summary>
        F4 = 4,
        /// <summary>
        ///  Amount, settlement
        /// </summary>
        F5 = 5,
        /// <summary>
        ///  Amount, cardholder billing
        /// </summary>
        F6 = 6,
        /// <summary>
        ///  Transmission date and time
        /// </summary>
        F7 = 7,
        /// <summary>
        ///  Amount, cardholder billing fee
        /// </summary>
        F8 = 8,
        /// <summary>
        ///  Conversion rate, settlement
        /// </summary>
        F9 = 9,
        /// <summary>
        ///  Conversion rate, cardholder billing
        /// </summary>
        F10 = 10,
        /// <summary>
        ///  System trace audit number
        /// </summary>
        F11 = 11,
        /// <summary>
        ///  Time, local transaction (hhmmss)
        /// </summary>
        F12 = 12,
        /// <summary>
        ///  Date, local transaction (MMDD)
        /// </summary>
        F13 = 13,
        /// <summary>
        ///  Date, expiration
        /// </summary>
        F14 = 14,
        /// <summary>
        ///  Date, Settlement
        /// </summary>
        F15 = 15,
        /// <summary>
        ///  Date, conversion
        /// </summary>
        F16 = 16,
        /// <summary>
        ///  Date, capture
        /// </summary>
        F17 = 17,
        /// <summary>
        ///  Merchant type
        /// </summary>
        F18 = 18,
        /// <summary>
        ///  Acquiring institution country code
        /// </summary>
        F19 = 19,
        /// <summary>
        ///  PAN extended, country code
        /// </summary>
        F20 = 20,
        /// <summary>
        ///  Forwarding institution. country code
        /// </summary>
        F21 = 21,
        /// <summary>
        ///  Point of service entry mode
        /// </summary>
        F22 = 22,
        /// <summary>
        ///  Application PAN sequence number
        /// </summary>
        F23 = 23,
        /// <summary>
        ///  Network International identifier (NII)
        /// </summary>
        F24 = 24,
        /// <summary>
        ///  Point of service entry mode
        /// </summary>
        F25 = 25,
        /// <summary>
        /// Point of service capture code
        /// </summary>
        F26 = 26,
        /// <summary>
        /// Authorizing identification response length
        /// </summary>
        F27 = 27,
        /// <summary>
        /// Amount, transaction fee
        /// </summary>
        F28 = 28,
        /// <summary>
        /// Amount, settlement fee
        /// </summary>
        F29 = 29,
        /// <summary>
        /// Amount, transaction processing fee
        /// </summary>
        F30 = 30,
        /// <summary>
        /// Amount, settlement processing fee
        /// </summary>
        F31 = 31,
        /// <summary>
        /// Acquiring institution identification code
        /// </summary>
        F32 = 32,
        /// <summary>
        /// Forwarding institution identification code
        /// </summary>
        F33 = 33,
        /// <summary>
        /// Primary account number, extended
        /// </summary>
        F34 = 34,
        /// <summary>
        /// Track 2 data
        /// </summary>
        F35 = 35,
        /// <summary>
        /// Track 3 data
        /// </summary>
        F36 = 36,
        /// <summary>
        /// Retrieval reference number
        /// </summary>
        F37 = 37,
        /// <summary>
        /// Authorization identification response
        /// </summary>
        F38 = 38,
        /// <summary>
        /// Response code
        /// </summary>
        F39 = 39,
        /// <summary>
        /// Service restriction code
        /// </summary>
        F40 = 40,
        /// <summary>
        /// Card acceptor terminal identification
        /// </summary>
        F41 = 41,
        /// <summary>
        /// Card acceptor identification code
        /// </summary>
        F42 = 42,
        /// <summary>
        /// Card acceptor name/location
        /// </summary>
        F43 = 43,
        /// <summary>
        /// Additional response data
        /// </summary>
        F44 = 44,
        /// <summary>
        /// Track1
        /// </summary>
        F45 = 45,
        /// <summary>
        /// Additional response data
        /// </summary>
        F46 = 46,
        /// <summary>
        /// Additional data - national
        /// </summary>
        F47 = 47,
        /// <summary>
        /// Additional data - Private
        /// </summary>
        F48 = 48,
        /// <summary>
        /// Currency code, transaction
        /// </summary>
        F49 = 49,
        /// <summary>
        /// Currency code, settlement
        /// </summary>
        F50 = 50,
        /// <summary>
        /// Currency code, cardholder billing
        /// </summary>
        F51 = 51,
        /// <summary>
        /// Personal identification number data
        /// </summary>
        F52 = 52,
        /// <summary>
        /// Security related control information
        /// </summary>
        F53 = 53,
        /// <summary>
        /// Additional amounts
        /// </summary>
        F54 = 54,
        /// <summary>
        /// Reserved ISO
        /// </summary>
        F55 = 55,
        /// <summary>
        /// Reserved ISO
        /// </summary>
        F56 = 56,
        /// <summary>
        /// Reserved national
        /// </summary>
        F57 = 57,
        /// <summary>
        /// Reserved national
        /// </summary>
        F58 = 58,
        /// <summary>
        /// Reserved national
        /// </summary>
        F59 = 59,
        /// <summary>
        /// Reserved national
        /// </summary>
        F60 = 60,
        /// <summary>
        /// Reserved private
        /// </summary>
        F61 = 61,
        /// <summary>
        /// Reserved private
        /// </summary>
        F62 = 62,
        /// <summary>
        /// Reserved private
        /// </summary>
        F63 = 63,
        /// <summary>
        /// Message authentication code (MAC)
        /// </summary>
        F64 = 64,
        /// <summary>
        /// Bitmap, extended
        /// </summary>
        F65 = 65,
        /// <summary>
        /// Settlement code
        /// </summary>
        F66 = 66,
        /// <summary>
        /// Extended payment code
        /// </summary>
        F67 = 67,
        /// <summary>
        /// Receiving institution country code
        /// </summary>
        F68 = 68,
        /// <summary>
        /// Settlement institution country code
        /// </summary>
        F69 = 69,
        /// <summary>
        /// Network management information code
        /// </summary>
        F70 = 70,
        /// <summary>
        /// Message number
        /// </summary>
        F71 = 71,
        /// <summary>
        /// Message number, last
        /// </summary>
        F72 = 72,
        /// <summary>
        /// Date, action (YYMMDD)
        /// </summary>
        F73 = 73,
        /// <summary>
        /// Credits, number
        /// </summary>
        F74 = 74,
        /// <summary>
        /// Credits, reversal number
        /// </summary>
        F75 = 75,
        /// <summary>
        /// Debits, number
        /// </summary>
        F76 = 76,
        /// <summary>
        /// Debits, reversal number
        /// </summary>
        F77 = 77,
        /// <summary>
        /// Transfer number
        /// </summary>
        F78 = 78,
        /// <summary>
        /// Transfer, reversal number
        /// </summary>
        F79 = 79,
        /// <summary>
        /// Inquiries number
        /// </summary>
        F80 = 80,
        /// <summary>
        /// Authorizations, number
        /// </summary>
        F81 = 81,
        /// <summary>
        /// Credits, processing fee amount
        /// </summary>
        F82 = 82,
        /// <summary>
        /// Credits, transaction fee amount
        /// </summary>
        F83 = 83,
        /// <summary>
        /// Debits, processing fee amount
        /// </summary>
        F84 = 84,
        /// <summary>
        /// Debits, transaction fee amount
        /// </summary>
        F85 = 85,
        /// <summary>
        /// Credits, amount
        /// </summary>
        F86 = 86,
        /// <summary>
        /// Credits, reversal amount
        /// </summary>
        F87 = 87,
        /// <summary>
        /// Debits, amount
        /// </summary>
        F88 = 88,
        /// <summary>
        /// Debits, reversal amount
        /// </summary>
        F89 = 89,
        /// <summary>
        /// Original data elements
        /// </summary>
        F90 = 90,
        /// <summary>
        /// File update code
        /// </summary>
        F91 = 91,
        /// <summary>
        /// File security code
        /// </summary>
        F92 = 92,
        /// <summary>
        /// Response indicator
        /// </summary>
        F93 = 93,
        /// <summary>
        /// Service indicator
        /// </summary>
        F94 = 94,
        /// <summary>
        /// Replacement amounts
        /// </summary>
        F95 = 95,
        /// <summary>
        /// Message security code
        /// </summary>
        F96 = 96,
        /// <summary>
        /// Amount, net settlement
        /// </summary>
        F97 = 97,
        /// <summary>
        /// Payee
        /// </summary>
        F98 = 98,
        /// <summary>
        /// Settlement institution identification code
        /// </summary>
        F99 = 99,
        /// <summary>
        /// Receiving institution identification code
        /// </summary>
        F100 = 100,
        /// <summary>
        /// File name
        /// </summary>
        F101 = 101,
        /// <summary>
        /// Account identification 1
        /// </summary>
        F102 = 102,
        /// <summary>
        /// Account identification 2
        /// </summary>
        F103 = 103,
        /// <summary>
        /// Transaction description
        /// </summary>
        F104 = 104,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F105 = 105,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F106 = 106,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F107 = 107,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F108 = 108,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F109 = 109,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F110 = 110,
        /// <summary>
        /// Reserved for ISO use
        /// </summary>
        F111 = 111,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F112 = 112,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F113 = 113,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F114 = 114,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F115 = 115,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F116 = 116,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F117 = 117,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F118 = 118,
        /// <summary>
        /// Reserved for national use
        /// </summary>
        F119 = 119,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F120 = 120,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F121 = 121,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F122 = 122,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F123 = 123,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F124 = 124,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F125 = 125,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F126 = 126,
        /// <summary>
        /// Reserved for private use
        /// </summary>
        F127 = 127,
        /// <summary>
        /// Message authentication code
        /// </summary>
        F128 = 128,
    }
}
