using System.Text;

namespace CSharp8583.Tests
{
    /// <summary>
    /// Contant Values Used by Test Cases
    /// </summary>
    public static class ConstantValues
    {
        public static string ASCIIMessage = "01001038040008C000000000000044440000211042120529021000000001015JI091003000000000111111";
        public static string ASCIIMessageWithResField = "01000000000000C00002JI09100300000000011111105801041234020B22222222222030EPROPERTY TAXES040D1111111111111";
        public static string ASCIIMessageWithSecondaryBitmap = "01009038040008C0000000800000000000000000000044440000211042120529021000000001015JI091003000000000111111121212";
        public static string ASCIIMessageMissingF41 = "01001038040008C000000000000044440000211042120529021000000001015000000000111111";
        public static string ASCIIMessageTagLengthThree = "01000000000000C000020000000500000000000111203901004000002009FR44548810300522215040010";
        public static string ASCIIMessageTagLengthTwo = "01000000000000C0000200000005000000000001112035010400000209FR445488103052221504010";

        public static byte[] ASCIIBytes = Encoding.ASCII.GetBytes(ASCIIMessage);
        public static byte[] ASCIIBytesMissingF41 = Encoding.ASCII.GetBytes(ASCIIMessageMissingF41);
        public static byte[] ASCIIBytesWithSecondaryBitmap = Encoding.ASCII.GetBytes(ASCIIMessageWithSecondaryBitmap);
        public static byte[] ASCIIBytesWithResField = Encoding.ASCII.GetBytes(ASCIIMessageWithResField);
        public static byte[] ASCIIBytesTagLengthTwo = Encoding.ASCII.GetBytes(ASCIIMessageTagLengthTwo);
        public static byte[] ASCIIBytesTagLengthThree = Encoding.ASCII.GetBytes(ASCIIMessageTagLengthThree);

    }
}